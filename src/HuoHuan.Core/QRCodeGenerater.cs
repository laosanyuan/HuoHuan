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
        private Image _image = null!;
        #endregion

        public QRCodeGenerater(string folderPath)
        {
            this._folder = folderPath;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
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
                Bitmap bitmap = writer.Write(content);
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

    }
}
