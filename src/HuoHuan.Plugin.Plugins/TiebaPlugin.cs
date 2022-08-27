﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;
using static HuoHuan.Plugin.Plugins.TiebaConfig;

namespace HuoHuan.Plugin.Plugins
{
    public class TiebaPlugin : IPlugin
    {
        #region [Properties]
        public string Name => "百度贴吧";

        public ISpider Spider { get; init; } = new TiebaSpider();

        public bool IsNeedConfig => true;

        public IConfig Config { get; init; } = new TiebaConfig();
        #endregion

        public async Task Init() => await this.Spider.Init(this.Config);

        public bool IsValid()
        {
            return true;
        }
    }

    public class TiebaSpider : ISpider
    {
        #region [Fileds]
        private TiebaConfig _config = null!;
        private readonly HtmlParser _parser = new();

        private int _count;             // 爬取数量
        private double _progress;       // 爬取进度
        private DateTime _startTime;    // 爬取起始时间
        #endregion

        public SpiderStatus Status { get; set; } = SpiderStatus.Waiting;

        public event ProgressEventHandler ProgressStatusChanged = null!;
        public event CrawledEventHandler Crawled = null!;

        #region [Public Methods]
        public void Start()
        {
            if (this._config is not null
                && this.Status != SpiderStatus.Unknown
                && this.Status != SpiderStatus.Running)
            {
                this.Status = SpiderStatus.Running;
                _ = this.CrawlImage();
                this.NotifyStatusProgressChange();
            }
        }

        public void Pause()
        {
            if (this._config is not null && this.Status == SpiderStatus.Running)
            {
                this.Status = SpiderStatus.Paused;
                this.NotifyStatusProgressChange();
            }
        }

        public void Continue()
        {
            if (this._config is not null && this.Status == SpiderStatus.Paused)
            {
                this.Status = SpiderStatus.Running;
            }
        }

        public void Stop()
        {
            if (this._config is not null && this.Status != SpiderStatus.Unknown)
            {
                this.Status = SpiderStatus.Waiting;
            }
        }

        public async Task Init(IConfig config)
        {
            if (config is TiebaConfig tmp)
            {
                this.Status = SpiderStatus.Waiting;
                await config.Load();
                this._config = tmp;
            }
        }
        #endregion

        #region [Private Methods]
        // 爬取任务
        private async Task CrawlImage()
        {
            this._count = 0;
            this._progress = 0;
            this._startTime = DateTime.Now;

            var pageCount = this._config?.Config?.Sum(t => t.PageCount) ?? 0;
            var currentPage = 0;

            for (int i = 0; i < this._config?.Config?.Count; i++)
            {
                var page = this._config.Config[i];
                for (int j = 0; j < page.PageCount; j++)
                {
                    try
                    {
                        HashSet<string> urls = new();
                        int index = j * 50;

                        HttpClient client = new();
                        HttpUtil.SetHeaders(client);
                        string pageData = await client.GetStringAsync($"https://tieba.baidu.com/f?kw={page.Name}&ie=utf-8&pn={index}");
                        if (pageData?.Contains("百度安全验证") == true
                            || pageData?.Contains("网络不给力") == true)
                        {
                            this.Status = SpiderStatus.Error;
                            this.NotifyStatusProgressChange();
                            return;
                        }
                        IHtmlDocument doc = await this._parser.ParseDocumentAsync(pageData);
                        IHtmlCollection<IElement> tags = doc.QuerySelectorAll(".t_con.cleafix");
                        foreach (IElement tag in tags)
                        {
                            IHtmlCollection<IElement> images = tag.QuerySelectorAll(".threadlist_pic.j_m_pic");
                            for (int k = 0; k < images.Length; k++)
                            {
                                IElement image = images[k];
                                var url = image.GetAttribute("bpic");
                                if (url != null)
                                {
                                    this.Crawled?.Invoke(this, new CrawlEventArgs() { Url = url });
                                    this._count++;
                                }
                                if (this.Status == SpiderStatus.Paused)
                                {
                                    await Task.Delay(100);
                                }
                                else if (this.Status != SpiderStatus.Running)
                                {
                                    this.NotifyStatusProgressChange();
                                    return;
                                }
                            }
                        }
                        currentPage++;
                        this._progress = currentPage / (pageCount * 1.0);
                        this.NotifyStatusProgressChange();
                        await Task.Delay(100);
                    }
                    catch (Exception)
                    {
                        // log
                    }
                }
            }

            this.Status = SpiderStatus.Finished;
            this.NotifyStatusProgressChange();
        }

        // 通知当前运行状态
        private void NotifyStatusProgressChange()
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
        #endregion
    }

    public class TiebaConfig : BaseConfig<List<TiebaInfo>>
    {
        public override List<TiebaInfo> Config { get; protected set; } = new List<TiebaInfo>();

        public override string Name => nameof(TiebaConfig);

        public override void Reset()
        {
            var text = DefaultConfigProvider.GetFileByPath("DefaultConfigs/TiebaConfig.yaml");
            this.Config = this.Config = YamlUtil.Deserializer<List<TiebaInfo>>(text);
            base.Save();
        }

        [Serializable]
        public record TiebaInfo
        {
            /// <summary>
            /// 贴吧名称
            /// </summary>
            public string Name { get; init; } = null!;
            /// <summary>
            /// 爬取页数
            /// </summary>
            public int PageCount { get; init; }
        }
    }
}
