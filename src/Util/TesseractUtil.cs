using System;
using System.Drawing;
using System.IO;

namespace HuoHuan.Util
{
    class TesseractUtil
    {
        /// <summary>
        /// 识别获取Bitmap文字内容
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static string GetImageText(Bitmap bitmap)
        {
            using (var ocr = new Tesseract.TesseractEngine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"), "chi_sim", Tesseract.EngineMode.Default))
            {
                var pix = Tesseract.PixConverter.ToPix(new Bitmap(bitmap));
                using (var page = ocr.Process(pix))
                {
                    return page.GetText();
                }
            }
        }
    }
}
