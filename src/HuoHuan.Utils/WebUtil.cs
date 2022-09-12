using System.Diagnostics;

namespace HuoHuan.Utils
{
    public static class WebUtil
    {
        /// <summary>
        /// 默认浏览器打开网页链接
        /// </summary>
        /// <param name="url"></param>
        public static void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
            {
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }
    }
}
