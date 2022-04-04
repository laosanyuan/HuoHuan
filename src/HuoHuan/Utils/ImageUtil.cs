using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace HuoHuan.Utils
{
    class ImageUtil
    {
        /// <summary>
        /// 根据URL获取图像
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        internal static Bitmap GetBitmapFromUrl(string url)
        {
            Bitmap result = null!;
            try
            {
                WebRequest webreq = WebRequest.Create(url);
                WebResponse webres = webreq.GetResponse();
                using Stream stream = webres.GetResponseStream();
                result = (Bitmap)Image.FromStream(stream);
            }
            catch (Exception)
            {
                result = null!;
            }

            return result!;
        }

        /// <summary>
        /// 下载远程图片到本地
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        internal static void SaveImageFile(string url, string fileName)
        {
            WebClient client = new WebClient();
            client.DownloadFileAsync(new Uri(url), fileName);
        }

        /// <summary>
        /// 判断图片链接是否为二维码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static bool IsQRCode(string url, out string text)
        {
            text = null!;
            var reader = new ZXing.BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var image = ImageUtil.GetBitmapFromUrl(url);
            if (image == null)
            {
                return false;
            }
            var qr = reader.Decode(image);
            if (qr != null)
            {
                text = qr.Text;
                return true;
            }
            return false;
        }
    }
}
