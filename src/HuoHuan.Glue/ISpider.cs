using System;

namespace HuoHuan.Glue
{
    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
    public delegate void CrawledEventHandler(object sender, CrawlEventArgs e);

    public interface ISpider
    {
        public SpiderStatus Status { get; }

        #region [Events]
        /// <summary>
        /// 进度状态变更
        /// </summary>
        public event ProgressEventHandler ProgressStatusChanged;
        /// <summary>
        /// 爬取结果通知
        /// </summary>
        public event CrawledEventHandler Crawled;
        #endregion

        #region [Methods]
        /// <summary>
        /// 开始爬取
        /// </summary>
        public void Start();
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause();
        /// <summary>
        /// 继续
        /// </summary>
        public void Continue();
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop();
        #endregion
    }

    public class ProgressEventArgs : EventArgs
    {
        public int Count { get; init; }
        public int AllCount { get; init; }
        public SpiderStatus Status { get; init; }
    }

    public class CrawlEventArgs : EventArgs
    {
        public string Url { get; init; }
    }

    public enum SpiderStatus
    {
        Waiting,    // 等待开始
        Running,    // 运行中
        Paused,     // 暂停
        Stopped,    // 停止
        Finished,   // 完成
    }
}
