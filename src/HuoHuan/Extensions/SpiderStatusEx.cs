using HuoHuan.Plugin.Contracs;

namespace HuoHuan.Extensions
{
    internal static class SpiderStatusEx
    {
        /// <summary>
        /// 是否已结束运行
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool IsEnded(this SpiderStatus status)
        {
            if (status == SpiderStatus.Stopped
                || status == SpiderStatus.Unknown
                || status == SpiderStatus.Finished
                || status == SpiderStatus.Error
                || status == SpiderStatus.Waiting)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool IsRunning(this SpiderStatus status)
        {
            if (status == SpiderStatus.Running || status == SpiderStatus.Paused)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
