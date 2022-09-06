using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;

namespace HuoHuan.Plugin.Plugins
{
    /// <summary>
    /// 每天有群 https://www.inss.cn/
    /// </summary>
    public class InssPlugin : IPlugin
    {
        public string Name => "每天有群";
        public string Description => String.Empty;

        public ISpider Spider { get; init; } = new InssSpider();

        public bool IsNeedConfig => false;

        public IConfig Config { get; init; } = default!;


        public Task Init()
        {
            return Task.CompletedTask;
        }

        public bool IsValid()
        {
            return true;
        }
    }

    public class InssSpider : BaseSpider
    {
        public override Task Init(IConfig config)
        {
            return Task.CompletedTask;
        }

        protected override async Task CrawlImage()
        {
            using HttpClient client = new();
            HttpUtil.SetHeaders(client);
            for (int i = 0; i < 20; i++)
            {
                string pageData = await client.GetStringAsync($"https://www.inss.cn/qzyfx/list_12_{i + 1}/");
                if (String.IsNullOrEmpty(pageData))
                {
                    continue;
                }
                IHtmlDocument doc = await this._parser.ParseDocumentAsync(pageData!);
                IHtmlCollection<IElement> tags = doc.QuerySelectorAll(".cover.tyweb-display-block.tyweb-overflow-hidden");
                foreach (var tag in tags)
                {
                    // zd_标记水印
                    var tmpUrl = tag.LastElementChild?.GetAttribute("src")?.Replace("zd_", "");
                    var name = tag.LastElementChild?.GetAttribute("alt");
                    if (!string.IsNullOrEmpty(tmpUrl))
                    {
                        base.NotifyCrawledChange(new CrawlEventArgs() { Url = "https://www.inss.cn" + tmpUrl, Name = name! });
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
                base._progress = i / 20.0;
                base.NotifyStatusProgressChange();
            }
            base.Status = SpiderStatus.Finished;
            base._progress = 1.0;
            base.NotifyStatusProgressChange();
        }
    }
}
