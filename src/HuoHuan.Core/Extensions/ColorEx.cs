using System.Drawing;

namespace HuoHuan.Core.Extensions
{
    internal static class ColorEx
    {
        /// <summary>
        /// 判断相同颜色
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsSame(this Color target, Color source)
        {
            return target.A == source.A
                && target.R == source.R
                && target.G == source.G
                && target.B == source.B;
        }
    }
}
