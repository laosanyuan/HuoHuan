using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using ZXing;
using Image = System.Drawing.Image;

namespace HuoHuan.Utils
{
    public static class ImageUtil
    {
        private static BarcodeReader _barcodeReader = new() { Options = new ZXing.Common.DecodingOptions() { CharacterSet = "UTF-8" } };

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
        /// TODO 应将截取图片和读取二维码拆分开
        /// </summary>
        /// <param name="image"></param>
        /// <returns>(是否为二维码，二维码内容，截取二维码下部分文字内容图片部分)</returns>
        [SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public static (bool IsQRCode, string Message, Bitmap Bitmap) IsQRCode(Bitmap image)
        {
            if (image is not null)
            {
                var reader = new BarcodeReader();
                reader.Options.CharacterSet = "UTF-8";
                var qr = reader.Decode(image);
                if (qr is not null)
                {
                    var startY = (int)(qr.ResultPoints[0].Y * 2 - qr.ResultPoints[3].Y) + 10;
                    return (true, qr.Text, CropBitmap(image, startY));
                }
            }
            return (false, default!, default!);
        }

        /// <summary>
        /// 裁剪提示信息图片
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="startY"></param>
        /// <returns></returns>
        [SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public static Bitmap CropBitmap(Bitmap bitmap, int startY)
        {
            Rectangle cropRect = new(0, startY, bitmap.Width, bitmap.Height - startY);
            return bitmap.Clone(cropRect, bitmap.PixelFormat);
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
        public static Bitmap ConvertToBpp(Bitmap bmp, byte threshold = 40)
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
