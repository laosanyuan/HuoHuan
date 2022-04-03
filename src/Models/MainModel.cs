namespace HuoHuan.Models
{
    public class CrawlInfo
    {
        /// <summary>
        /// 当前链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 已爬取数量
        /// </summary>
        public int CrawledCount { get; set; }
        /// <summary>
        /// 当前下载进度
        /// </summary>
        public double Process { get; set; }
    }

    public class DownloadInfo
    {
        /// <summary>
        /// 当前链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 已下载数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int LaveCount { get; set; }
    }

    public class DisplayImageInfo
    {
        public string Url { get; set; }
        public bool IsValid { get; set; }
        public bool IsDownloaded { get; set; }
    }
}
