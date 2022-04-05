using System;

#nullable disable

namespace HuoHuan.Models
{
    /// <summary>
    /// 下载事件
    /// </summary>
    internal class DownloadEventArgs : EventArgs
    {
        /// <summary>
        /// 已下载数量
        /// </summary>
        public int DownloadedCount { get; set; }
        /// <summary>
        /// 剩余未下载数量
        /// </summary>
        public int LaveCount { get; set; }
        /// <summary>
        /// 群信息
        /// </summary>
        public GroupData GroupData { get; set; }
    }

    /// <summary>
    /// 爬取事件
    /// </summary>
    internal class CrawlEventArgs : EventArgs
    {
        /// <summary>
        /// 爬取数量
        /// </summary>
        public int CrawledCount { get; set; }
        /// <summary>
        /// 爬取进度
        /// </summary>
        public double Process { get; set; }
        /// <summary>
        /// 群信息
        /// </summary>
        public GroupData GroupData { get; set; }
        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool IsFinish { get; set; }
        /// <summary>
        /// 是否为有效二维码图片
        /// </summary>
        public bool IsValidImage { get; set; }
    }
}
