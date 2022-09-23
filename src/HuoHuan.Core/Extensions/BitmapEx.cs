using System.Drawing;

namespace HuoHuan.Core.Extensions
{
    internal static class BitmapEx
    {
        /// <summary>
        /// 更改Bitmap透明度
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="alpha"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public static void ToTransparent(this Bitmap bitmap, double rate)
        {
            if (rate < 0)
            {
                rate = 0;
            }
            else if (rate > 1)
            {
                rate = 1;
            }

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    bitmap.SetPixel(i, j, Color.FromArgb((byte)(color.A * rate), color));
                }
            }
        }

        /// <summary>
        /// 替换Bitmap特性颜色
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="replace"></param>
        /// <param name="target"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public static void ReplaceColor(this Bitmap bitmap, Color replace, Color target)
        {
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);

                    if (color.IsSame(target))
                    {
                        bitmap.SetPixel(i, j, replace);
                    }
                }
            }
        }
    }
}
