using HuoHuan.DataBase.Models;
using HuoHuan.DataBase.Services;
using HuoHuan.Utils;
using PaddleOCRSharp;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HuoHuan.Plugin
{
    /// <summary>
    /// 群二维码图片过滤
    /// </summary>
    internal class GroupFilter
    {
        #region [Fields]
        private readonly string _urlFlag = "https://weixin.qq.com/g/";  // 微信群链接标记
        private readonly CrawledImageDB _db = new();
        private readonly PaddleOCREngine _engine = new(null, new OCRParameter());
        private object _sync = new();
        #endregion

        #region [Methods]
        /// <summary>
        /// 是否为重复图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> IsRepeatImage(string url) => await this._db.IsExistsAndInsert(url);

        /// <summary>
        /// 判断url是否为有效图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<(bool IsValidate, string Message)> IsValidImage(string url)
        {
            // 1. 判断url是否重复
            if (!await this._db.IsExistsAndInsert(url))
            {
                // 2. 判断是否为二维码
                return await ImageUtil.IsQRCode(url);
            }
            return (false, null!);
        }

        /// <summary>
        /// 获取群数据
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<GroupImage> GetGroupData(string imageUrl, string text)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(text) && text.Contains(this._urlFlag))
                {
                    var image = await ImageUtil.GetBitmapFromUrl(imageUrl);
                    if (image is null)
                    {
                        return default!;
                    }
                    // 图像预处理
                    image = ImageUtil.ToGray(image);
                    image = ImageUtil.ConvertToBpp(image, 20);
                    // 获取图片文字内容
                    var dateStr = this.GetImageText(image).Replace(" ", "");
                    string pattern = @"内\((.+)前\)";

                    if (!String.IsNullOrWhiteSpace(dateStr)
                        && Regex.Matches(dateStr, pattern).Count > 0
                        && DateTime.TryParse(dateStr[..dateStr.LastIndexOf("前")].Split('(')[1], out var date)
                        && DateTimeUtil.IsValidTime(DateTime.Now, date, 7))
                    {
                        var result = new GroupImage()
                        {
                            InvalidateDate = date,
                            QRText = text,
                            Url = imageUrl,
                            GroupName = dateStr.Contains("该二维码") ? dateStr.Split("该二维码")?[0] : String.Empty,
                            FileName = text.Replace(this._urlFlag, "") + ".jpg"
                        };
                        return result;
                    }
                }
            }
            catch (Exception)
            {
            }
            return null!;
        }

        /// <summary>
        /// 识别获取Bitmap文字内容
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private string GetImageText(Bitmap bitmap)
        {
            try
            {
                lock (_sync)
                {
                    var ocrResult = _engine.DetectText(bitmap);
                    return ocrResult.Text;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion
    }
}
