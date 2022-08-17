using System.Drawing;
using Image = System.Drawing.Image;

namespace HuoHuan.Utils
{
    public class ImageUtil
    {
        /// <summary>
        /// 根据URL获取图像
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static async Task<Bitmap> GetBitmapFromUrl(string url)
        {
            Bitmap result = null!;
            try
            {
                HttpClient httpClient = new();
                using Stream stream = await httpClient.GetStreamAsync(url);
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
        public static async Task SaveImageFile(string url, string fileName)
        {
            HttpClient client = new();
            byte[] bytes = await client.GetByteArrayAsync(url);
            using FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 判断图片链接是否为二维码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async Task<(bool, string)> IsQRCode(string url)
        {
            var reader = new ZXing.BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var image = ImageUtil.GetBitmapFromUrl(url).Result;
            if (image == null)
            {
                return (false, null!);
            }
            var qr = reader.Decode(image);
            if (qr != null)
            {
                return (true, qr.Text);
            }
            return (false, null!);
        }

    }
}
