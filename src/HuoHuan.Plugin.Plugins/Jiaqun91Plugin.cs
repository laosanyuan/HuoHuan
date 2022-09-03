using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;

namespace HuoHuan.Plugin.Plugins
{
    /// <summary>
    /// https://www.91jiaqun.com/
    /// </summary>
    public class Jiaqun91Plugin : IPlugin
    {
        public string Name => "91jiaqun";

        public ISpider Spider { get; init; } = new Jiaqun91Spider();

        public bool IsNeedConfig => false;

        public IConfig Config { get; init; } = null!;

        public Task Init()
        {
            return Task.CompletedTask;
        }

        public bool IsValid()
        {
            return true;
        }
    }

    public class Jiaqun91Spider : BaseSpider
    {
        public override Task Init(IConfig config)
        {
            return Task.CompletedTask;
        }

        protected override async Task CrawlImage()
        {
            using HttpClient client = new();
            HttpUtil.SetHeaders(client);
            for (int i = 1; i < 500; i++)
            {
                string pageData = await client.GetStringAsync($"https://www.91jiaqun.com/page/{i}");
                if (String.IsNullOrEmpty(pageData))
                {
                    continue;
                }
                IHtmlDocument doc = await this._parser.ParseDocumentAsync(pageData!);
                IHtmlCollection<IElement> tags = doc.QuerySelectorAll(".gridhub-grid-post.gridhub-5-col");
                foreach (var tag in tags)
                {
                    var url = tag.QuerySelector(".gridhub-grid-post-thumbnail-img")?.GetAttribute("data-src");
                    var name = tag.QuerySelector(".gridhub-grid-post-thumbnail-img")?.GetAttribute("title");
                    var dateStr = tag.QuerySelector(".gridhub-grid-post-date.gridhub-grid-post-meta")?.InnerHtml;
                    if (DateTime.TryParse(dateStr, out var date))
                    {
                        if (!DateTimeUtil.IsValidTime(DateTime.Now, date.AddDays(7), 7))
                        {
                            goto _End;
                        }
                    }
                    if (!string.IsNullOrEmpty(url))
                    {
                        // 无法获知确切失效时间，保守1天
                        base.NotifyCrawledChange(
                            new CrawlEventArgs()
                            {
                                Url = url,
                                Name = name!,
                                NeedFilter = false,
                                InvalidTime = date.AddDays(6)
                            });
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
                base._progress = i / 500.0;
                base.NotifyStatusProgressChange();
            }
        _End:
            base.Status = SpiderStatus.Finished;
            base._progress = 1.0;
            base.NotifyStatusProgressChange();
        }
    }
}
