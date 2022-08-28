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
#pragma warning disable CA1416 // 验证平台兼容性
                result = (Bitmap)Image.FromStream(stream);
#pragma warning restore CA1416 // 验证平台兼容性
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
            try
            {
                HttpClient client = new();
                byte[] bytes = await client.GetByteArrayAsync(url);
                using FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception)
            { 
            }
        }

        /// <summary>
        /// 判断图片链接是否为二维码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async Task<(bool IsQRCode, string Message)> IsQRCode(string url)
        {
            var reader = new ZXing.BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var image = await GetBitmapFromUrl(url);
            if (image == null)
            {
                return (false, null!);
            }
#pragma warning disable CA1416 // 验证平台兼容性
            var qr = reader.Decode(image);
#pragma warning restore CA1416 // 验证平台兼容性
            if (qr != null)
            {
                return (true, qr.Text);
            }
            return (false, null!);
        }

    }
}
