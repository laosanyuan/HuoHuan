using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;

namespace HuoHuan.Plugin.Plugins
{
    /// <summary>
    /// 免费微信群 http://mfwxq.com/
    /// </summary>
    public class MfwxqPlugin : IPlugin
    {
        public string Name => "免费微信群";

        public string Description => String.Empty;

        public ISpider Spider { get; init; } = new MfwxqSpider();

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

    public class MfwxqSpider : BaseSpider
    {
        protected override async Task CrawlImage()
        {
            using HttpClient client = new();
            HttpUtil.SetHeaders(client);
            for (int i = 0; i < 15; i++)
            {
                string pageData = await client.GetStringAsync($"http://mfwxq.com/Product.asp?PicClassID=&page={i + 1}/");
                if (String.IsNullOrEmpty(pageData))
                {
                    continue;
                }
                IHtmlDocument doc = await this._parser.ParseDocumentAsync(pageData!);
                IHtmlCollection<IElement> tags = doc.QuerySelectorAll(".pro_list");
                foreach (var tag in tags)
                {
                    var imageTag = tag.QuerySelector("div a img");
                    var title = imageTag?.GetAttribute("alt");

                    if (string.IsNullOrEmpty(title))
                    {
                        continue;
                    }

                    var url = $"http://mfwxq.com{imageTag?.GetAttribute("data-original")}";
                    var datetime = Convert.ToDateTime(tag.QuerySelector(".pro_title.gray")?.TextContent?.Replace("发布日期", "")).AddDays(6);

                    if (datetime < DateTime.Now)
                    {
                        continue;
                    }

                    base.NotifyCrawledChange(new CrawlEventArgs() { Url = url, Name = title, InvalidTime = datetime.AddDays(6) });

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
                base._progress = i / 15.0;
                base.NotifyStatusProgressChange();
            }
            base.Status = SpiderStatus.Finished;
            base._progress = 1.0;
            base.NotifyStatusProgressChange();
        }
    }
}
