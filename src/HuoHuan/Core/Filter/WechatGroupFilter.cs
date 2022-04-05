using HuoHuan.Data.DataBase;
using HuoHuan.Enums;
using HuoHuan.Models;
using HuoHuan.Utils;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HuoHuan.Core.Filter
{
    /// <summary>
    /// 微信群筛选
    /// </summary>
    class WechatGroupFilter : IGroupFilter
    {
        private readonly string urlFlag = "https://weixin.qq.com/g/";  // 微信群链接标记
        private readonly GroupDB db;    // 微信群链接数据库
        private string text = null!;            // 二维码内容

        public WechatGroupFilter()
        {
            this.db = new GroupDB("wechat_group");
        }

        public QRCodeType FilterType { get; } = QRCodeType.WechatGroup;

        public GroupData GetGroupData(string imageUrl)
        {
            if (!String.IsNullOrWhiteSpace(this.text))
            {

                HttpClient httpClient = new HttpClient();
                HttpUtil.SetHeaders(httpClient);
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Microsoft Internet Explorer");
                httpClient.BaseAddress = new Uri(imageUrl);

                using Stream s = httpClient.GetStreamAsync(imageUrl).Result;
                byte[] data = new byte[1024];
                int length = 0;
                using MemoryStream ms = new();
                while ((length = s.Read(data, 0, data.Length)) > 0)
                {
                    ms.Write(data, 0, length);
                }
                ms.Seek(0, SeekOrigin.Begin);

                // 降噪
                Mat simg = Mat.FromStream(ms, ImreadModes.Grayscale);
                // 二值化
                Mat thresholdImg = simg.Threshold(210, 255, ThresholdTypes.Binary);
                // 获取图片文字内容
                var dateStr = PaddleUtil.GetImageText(BitmapConverter.ToBitmap(thresholdImg)).Replace(" ", "");
                if (!String.IsNullOrWhiteSpace(dateStr))
                {
                    string pattern = @"内\((.+)前\)";
                    if (Regex.Matches(dateStr, pattern).Count > 0)
                    {
                        if (DateTime.TryParse(dateStr[..dateStr.LastIndexOf("前")].Split('(')[1], out var date))
                        {
                            bool isValid = DateTimeUtil.IsValidTime(DateTime.Now, date, 7);
                            if (isValid)
                            {
                                var result = new GroupData()
                                {
                                    InvalidateDate = date,
                                    QRText = this.text,
                                    SourceUrl = imageUrl,
                                    FileName = this.text.Replace(this.urlFlag, "")
                                };
                                return result;
                            }
                        }
                    }
                }
            }
            return null!;
        }

        public async Task<bool> IsValidImage(string imageUrl)
        {
            // 1.判断Url数据库是否存在/标记
            var isUsed = await UrlDB.Instance.IsUsedUrl(imageUrl, this.db.TableName);
            if (!isUsed)
            {
                // 2.判断当前群数据库是否存在/标记
                bool isExist = await this.db.IsExistUrl(imageUrl);

                // 3.判断是否为二维码及有效
                if (!isExist && ImageUtil.IsQRCode(imageUrl, out var text) && text.Contains(this.urlFlag))
                {
                    this.text = text;
                    // 4.更新链接标记
                    UrlDB.Instance.UpdateUsedUrl(imageUrl, this.db.TableName);
                    return true;
                }
            }
            this.text = String.Empty;
            return false;
        }

        public void SaveData(GroupData data)
        {
            this.db.InsertGroup(data);
        }
    }
}
