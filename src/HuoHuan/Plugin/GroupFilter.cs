using HuoHuan.Data.DataBase;
using HuoHuan.Glue.Utils;
using HuoHuan.Models;
using HuoHuan.Utils;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HuoHuan.Plugin
{
    /// <summary>
    /// 群二维码图片过滤
    /// </summary>
    internal class GroupFilter
    {
        private readonly string _urlFlag = "https://weixin.qq.com/g/";  // 微信群链接标记

        public async Task<bool> IsValidImage(string url)
        {
            // 1. 判断url是否重复
            if (!await CrawledImageDB.Instance.IsExistsAndInsert(url))
            {

                // 2. 判断是否为二维码
                if (ImageUtil.IsQRCode(url, out string text))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取群数据
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public GroupData GetGroupData(string imageUrl, string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
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
                string pattern = @"内\((.+)前\)";

                if (!String.IsNullOrWhiteSpace(dateStr)
                    && Regex.Matches(dateStr, pattern).Count > 0
                    && DateTime.TryParse(dateStr[..dateStr.LastIndexOf("前")].Split('(')[1], out var date)
                    && DateTimeUtil.IsValidTime(DateTime.Now, date, 7))
                {
                    var result = new GroupData()
                    {
                        InvalidateDate = date,
                        QRText = text,
                        SourceUrl = imageUrl,
                        FileName = text.Replace(this._urlFlag, "")
                    };
                    return result;
                }
            }
            return null!;
        }
    }
}
