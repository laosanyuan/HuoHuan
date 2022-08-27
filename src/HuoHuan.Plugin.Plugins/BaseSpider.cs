using AngleSharp.Html.Parser;
using HuoHuan.Plugin.Contracs;

namespace HuoHuan.Plugin.Plugins
{
    public abstract class BaseSpider : ISpider
    {
        #region [Fileds]
        protected readonly HtmlParser _parser = new();  // html解析
        protected int _count;                           // 爬取数量
        protected double _progress;                     // 爬取进度
        protected DateTime _startTime;                  // 爬取起始时间
        #endregion

        public SpiderStatus Status { get; protected set; } = SpiderStatus.Waiting;

        public event ProgressEventHandler ProgressStatusChanged = null!;
        public event CrawledEventHandler Crawled = null!;

        #region [Public Methods]
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public abstract Task Init(IConfig config);

        /// <summary>
        /// 开始
        /// </summary>
        public virtual void Start()
        {
            if (this.Status != SpiderStatus.Unknown && this.Status != SpiderStatus.Running)
            {
                this.Status = SpiderStatus.Running;
                this._count = 0;
                this._progress = 0;
                this._startTime = DateTime.Now;

                Task.Run(this.CrawlImage);
                this.NotifyStatusProgressChange();
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public virtual void Pause()
        {
            if (this.Status == SpiderStatus.Running)
            {
                this.Status = SpiderStatus.Paused;
                this.NotifyStatusProgressChange();
            }
        }

        /// <summary>
        /// 继续
        /// </summary>
        public virtual void Continue()
        {
            if (this.Status == SpiderStatus.Paused)
            {
                this.Status = SpiderStatus.Running;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        {
            if (this.Status != SpiderStatus.Unknown)
            {
                this.Status = SpiderStatus.Waiting;
            }
        }
        #endregion

        #region [Private Methods]
        /// <summary>
        /// 更新进度变化
        /// </summary>
        protected virtual void NotifyStatusProgressChange()
        {
            var used = DateTime.Now - this._startTime;
            var left = this._progress <= 0 ? default : used / this._progress - used;

            this.ProgressStatusChanged?.Invoke(this,
                new ProgressEventArgs()
                {
                    Count = this._count,
                    Status = this.Status,
                    Progress = this._progress,
                    UsedTime = used,
                    LeftTime = left
                });
        }

        /// <summary>
        /// 更新爬取链接
        /// </summary>
        /// <param name="url"></param>
        protected virtual void NotifyCrawledChange(CrawlEventArgs crawl)
        {
            this.Crawled?.Invoke(this, crawl);
            this._count++;
        }

        protected abstract Task CrawlImage();
        #endregion
    }
}
