using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;
using static HuoHuan.Plugin.Plugins.TiebaConfig;

namespace HuoHuan.Plugin.Plugins
{
    /// <summary>
    /// 百度贴吧
    /// </summary>
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

    public class TiebaSpider : BaseSpider
    {
        private TiebaConfig _config = null!;    //配置

        #region [Public Methods]
        public override void Start()
        {
            if (this._config is not null)
            {
                base.Start();
            }
        }

        public override void Pause()
        {
            if (this._config is not null)
            {
                base.Pause();
            }
        }

        public override void Continue()
        {
            if (this._config is not null)
            {
                base.Continue();
            }
        }

        public override void Stop()
        {
            if (this._config is not null)
            {
                base.Stop();
            }
        }

        public override async Task Init(IConfig config)
        {
            if (config is TiebaConfig tmp)
            {
                base.Status = SpiderStatus.Waiting;
                await config.Load();
                this._config = tmp;
            }
        }
        #endregion

        #region [Private Methods]
        // 爬取任务
        protected override async Task CrawlImage()
        {
            var pageCount = this._config?.Config?.Sum(t => t.PageCount) ?? 0;
            var currentPage = 0;
            HttpClient client = new();
            HttpUtil.SetHeaders(client);

            for (int i = 0; i < this._config?.Config?.Count; i++)
            {
                var page = this._config.Config[i];
                for (int j = 0; j < page.PageCount; j++)
                {
                    try
                    {
                        HashSet<string> urls = new();
                        int index = j * 50;

                        string pageData = await client.GetStringAsync($"https://tieba.baidu.com/f?kw={page.Name}&ie=utf-8&pn={index}");
                        if (pageData is null)
                        {
                            continue;
                        }
                        if (pageData?.Contains("百度安全验证") == true
                            || pageData?.Contains("网络不给力") == true)
                        {
                            this.Status = SpiderStatus.Error;
                            this.NotifyStatusProgressChange();
                            return;
                        }
                        IHtmlDocument doc = await this._parser.ParseDocumentAsync(pageData!);
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
                                    base.NotifyCrawledChange(new CrawlEventArgs() { Url = url });
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
                        base._progress = currentPage / (pageCount * 1.0);
                        base.NotifyStatusProgressChange();
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
