using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;

namespace HuoHuan.Plugin.Plugins
{
    /// <summary>
    /// wwqun.com
    /// </summary>
    public class WwqunPlugin : IPlugin
    {
        public string Name => "wwqun";

        public ISpider Spider { get; init; } = new WwqunSpider();

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

    public class WwqunSpider : BaseSpider
    {
        public override Task Init(IConfig config)
        {
            return Task.CompletedTask;
        }

        protected override async Task CrawlImage()
        {
            HttpClient client = new();
            HttpUtil.SetHeaders(client);
            for (int i = 0; i < 10; i++)
            {
                string pageData = await client.GetStringAsync($"http://wwqun.com/Product.asp?PicClassID=0&page={i + 1}");
                if (String.IsNullOrEmpty(pageData))
                {
                    continue;
                }
                IHtmlDocument doc = await this._parser.ParseDocumentAsync(pageData!);
                IHtmlCollection<IElement> tags = doc.QuerySelectorAll(".pro_list");
                foreach (IElement tag in tags)
                {
                    var src = tag.QuerySelector(".pro_img")?.GetElementsByTagName("img")?.FirstOrDefault();
                    var tmpUrl = src?.GetAttribute("data-original");
                    var name = src?.GetAttribute("alt");
                    var dateStr = tag.QuerySelector(".pro_title.gray")?.InnerHtml?.Replace("发布日期", "");

                    if (DateTime.TryParse(dateStr, out var time))
                    {
                        time = time.AddDays(7);
                        if (time < DateTime.Now)
                        {
                            base._progress = 1.0;
                            break;
                        }
                        if (!String.IsNullOrEmpty(tmpUrl))
                        {
                            base.NotifyCrawledChange(new CrawlEventArgs()
                            {
                                Url = "http://wwqun.com" + tmpUrl,
                                Name = name!,
                                InvalidTime = time,
                                NeedFilter = false
                            });
                        }
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
                base._progress = (i + 1) / (10.0);
                base.NotifyStatusProgressChange();
            }
            base.Status = SpiderStatus.Finished;
            base.NotifyStatusProgressChange();
        }
    }
}
