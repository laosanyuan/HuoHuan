using PaddleOCRSharp;
using System.Drawing;

namespace HuoHuan.Utils
{
    internal static class PaddleUtil
    {
        private static readonly PaddleOCREngine _engine = new(null, new OCRParameter());

        /// <summary>
        /// 识别获取Bitmap文字内容
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static string GetImageText(Bitmap bitmap)
        {
            var ocrResult = _engine.DetectText(bitmap);

            return ocrResult.Text;
        }
    }
}
