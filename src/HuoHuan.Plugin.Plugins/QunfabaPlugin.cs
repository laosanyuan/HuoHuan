using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;

namespace HuoHuan.Plugin.Plugins
{
    /// <summary>
    /// 群发吧
    /// </summary>
    public class QunfabaPlugin : IPlugin
    {
        public string Name => "群发吧";

        public string Description => String.Empty;

        public ISpider Spider { get; init; } = new QunfabaSpider();

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

    public class QunfabaSpider : BaseSpider
    {
        public override Task Init(IConfig config)
        {
            return Task.CompletedTask;
        }

        protected override async Task CrawlImage()
        {
            using HttpClient client = new();
            HttpUtil.SetHeaders(client);
            for (int i = 0; i < 10; i++)
            {
                string pageData = await client.GetStringAsync($"https://www.qunfaba.cn/forum-77-{i + 1}.html");
                if (String.IsNullOrEmpty(pageData))
                {
                    continue;
                }
                IHtmlDocument doc = await this._parser.ParseDocumentAsync(pageData!);
                IHtmlCollection<IElement> tags = doc.QuerySelectorAll(".s.xst");
                for (int j = 0; j < tags.Length; j++)
                {
                    var tag = tags[j];
                    var subUrl = tag.GetAttribute("href");
                    pageData = await client.GetStringAsync(subUrl);
                    if (String.IsNullOrEmpty(pageData))
                    {
                        continue;
                    }
                    doc = await this._parser.ParseDocumentAsync(pageData);
                    IHtmlCollection<IElement> imageTags = doc.QuerySelectorAll(".zoom");
                    foreach (var tmpTag in imageTags)
                    {
                        var url = "https://www.qunfaba.cn/" + tmpTag.GetAttribute("zoomfile");
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
                    base._progress = i / 10.0 + (j * 1.0 / tags.Length) / 10.0;
                    base.NotifyStatusProgressChange();
                }
            }
            base.Status = SpiderStatus.Finished;
            base._progress = 1.0;
            base.NotifyStatusProgressChange();
        }
    }
}
