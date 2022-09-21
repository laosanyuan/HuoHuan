using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace HuoHuan.Core
{
    /// <summary>
    /// 二维码生成
    /// </summary>
    public class QRCodeGenerater
    {
        #region [Fields]
        private string _folder;
        private Bitmap _logo = null!;
        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public QRCodeGenerater(string folderPath, string logo)
        {
            this._folder = folderPath;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (!string.IsNullOrEmpty(logo) && File.Exists(logo))
            {
                this._logo = (Bitmap)Image.FromFile(logo);
            }
        }

        #region [Public Methods]
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public bool GenerateImage(string content, string fileName = null!)
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }
            fileName ??= content + ".jpg";

            try
            {
                if (fileName.LastIndexOf(".jpg") < 0)
                {
                    fileName += ".jpg";
                }

                string file = Path.Combine(this._folder, fileName);
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = 1000,
                        Width = 1000,
                        DisableECI = true,
                        CharacterSet = "UTF-8",
                        ErrorCorrection = ErrorCorrectionLevel.H,
                        Margin = 3,
                    },
                };
                Bitmap bitmap = OverlayLogoToQR(writer.Write(content));
                bitmap.Save(file, ImageFormat.Jpeg);
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                // log
                return false;
            }
            return true;
        }
        #endregion

        #region [Private Methods]
        /// <summary>
        /// 向二维码Bitmap中间绘制logo
        /// </summary>
        /// <param name="qr"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        private Bitmap OverlayLogoToQR(Bitmap qr)
        {
            if (this._logo is null)
            {
                return qr;
            }

            Graphics g = Graphics.FromImage(qr);
            Rectangle logoRec = new Rectangle
            {
                Width = qr.Width / 6,
                Height = qr.Height / 6
            };
            logoRec.X = qr.Width / 2 - logoRec.Width / 2;
            logoRec.Y = qr.Height / 2 - logoRec.Height / 2;
            g.DrawImage(_logo, logoRec);

            return qr;
        }
        #endregion

    }
}
