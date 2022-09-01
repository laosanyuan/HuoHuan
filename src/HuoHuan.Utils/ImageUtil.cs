using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public static async Task<Bitmap> GetBitmapFromUrl(string url)
        {
            Bitmap result = null!;
            try
            {
                var handler = new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    }
                };
                using HttpClient httpClient = new(handler);
                HttpUtil.SetHeaders(httpClient, false);
                httpClient.BaseAddress = new Uri(url);
                using Stream stream = await httpClient.GetStreamAsync(url);
                result = (Bitmap)Image.FromStream(stream);
            }
            catch (Exception ex)
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
                using HttpClient client = new();
                HttpUtil.SetHeaders(client, false);
                byte[] bytes = await client.GetByteArrayAsync(url);
                using FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 判断图片链接是否为二维码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public static async Task<(bool IsQRCode, string Message)> IsQRCode(string url)
        {
            var reader = new ZXing.BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var image = await GetBitmapFromUrl(url);
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

        /// <summary>
        /// 灰度
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        [SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public static Bitmap ToGray(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    //利用公式计算灰度值
                    int gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }

        /// <summary>
        /// 二值化
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="threshold">二值化分界阈值</param>
        /// <returns></returns>
        [SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public static Bitmap ConvertToBpp(Bitmap bmp, byte threshold = 20)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bmp.GetPixel(i, j);
                    int value = 255 - color.B;
                    Color newColor = value > threshold ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
                    bmp.SetPixel(i, j, newColor);
                }
            }
            return bmp;
        }
    }
}
