﻿namespace HuoHuan.Plugin.Contracs
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
        /// 初始化
        /// </summary>
        public Task Init(IConfig config);
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
        /// <summary>
        /// 获取数量
        /// </summary>
        public int Count { get; init; }
        /// <summary>
        /// 进度
        /// </summary>
        public double Progress { get; init; }
        /// <summary>
        /// 耗时
        /// </summary>
        public TimeSpan UsedTime { get; init; }
        /// <summary>
        /// 剩余耗时
        /// </summary>
        public TimeSpan LeftTime { get; init; }
        /// <summary>
        /// 爬取状态
        /// </summary>
        public SpiderStatus Status { get; init; }
    }

    public class CrawlEventArgs : EventArgs
    {
        /// <summary>
        /// 图片链接
        /// </summary>
        public string Url { get; init; } = null!;
        /// <summary>
        /// 群名称
        /// </summary>
        public string Name { get; init; } = null!;
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime InvalidTime { get; init; }
        /// <summary>
        /// 是否需要过滤图片
        /// </summary>
        public bool NeedFilter { get; init; } = true;
    }

    public enum SpiderStatus
    {
        Unknown,
        Waiting,    // 等待开始

        Running,    // 运行中
        Paused,     // 暂停

        Stopped,    // 停止
        Finished,   // 完成
        Error,      // 异常
    }
}
