using HuoHuan.Core.Extensions;
using HuoHuan.DataBase.Models;
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

        #region [Properties]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public FontFamily FontFamily { get; set; } = new FontFamily("Arial");
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
                this._logo.ToTransparent(0.4);
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
        public bool GenerateImage(GroupImage group, string fileName = null!)
        {
            if (group is null || string.IsNullOrEmpty(group.QRText))
            {
                return false;
            }
            fileName ??= group.QRText + ".jpg";

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
                        Height = 2000,
                        Width = 2000,
                        DisableECI = true,
                        CharacterSet = "UTF-8",
                        ErrorCorrection = ErrorCorrectionLevel.H,
                        Margin = 3,
                    },
                };
                Bitmap bitmap = writer.Write(group.QRText);

                Color color = ColorTranslator.FromHtml("#4FE6AF");
                // 二维码染色
                bitmap.ReplaceColor(color, Color.Black);
                // 覆盖图标
                OverlayLogoToQR(bitmap);
                // 写信息
                bitmap = WriteMessage(bitmap, color, group.GroupName!, group.InvalidateDate);

                // 保存图片
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        private void OverlayLogoToQR(Bitmap qr)
        {
            if (this._logo is null)
            {
                return;
            }

            Graphics g = Graphics.FromImage(qr);
            Rectangle logoRec = new()
            {
                Width = qr.Width / 5,
                Height = qr.Height / 5
            };
            logoRec.X = qr.Width / 2 - logoRec.Width / 2;
            logoRec.Y = qr.Height / 2 - logoRec.Height / 2;
            g.DrawImage(_logo, logoRec);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        private Bitmap WriteMessage(Bitmap qr, Color color, string name, DateTime invalidData)
        {
            Bitmap bitmap = new(qr.Width, qr.Height + 300);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            var qrRectangle = new Rectangle(0, 0, qr.Width, qr.Height);
            graphics.DrawImage(qr, qrRectangle);

            graphics.DrawString($"群名：{name}",
                new Font(this.FontFamily, 75, FontStyle.Bold, GraphicsUnit.Pixel),
                new SolidBrush(color),
                new PointF(85.0f, qr.Height - 50));
            graphics.DrawString($"有效截至：{invalidData:yyyy年MM月dd日}",
                new Font(this.FontFamily, 75, FontStyle.Bold, GraphicsUnit.Pixel),
                new SolidBrush(color),
                new PointF(85.0f, qr.Height + 50));
            graphics.DrawString("项目地址：https://github.com/laosanyuan/HuoHuan",
                new Font(this.FontFamily, 60, FontStyle.Bold, GraphicsUnit.Pixel),
                new SolidBrush(color),
                new PointF(95.0f, qr.Height + 160));

            return bitmap;
        }
        #endregion
    }
}
