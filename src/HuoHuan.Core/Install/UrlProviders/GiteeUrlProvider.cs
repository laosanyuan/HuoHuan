using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace HuoHuan.Core.Install.UrlProviders
{
    public class GiteeUrlProvider : IUrlPriovider
    {
        public async Task<string> GetDownloadUrl(string url, string version)
        {
            string result = null!;
            try
            {
                using HttpClient client = new();
                var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                var pageData = await response.Content.ReadAsStringAsync();

                HtmlParser parser = new();
                IHtmlDocument doc = await parser.ParseDocumentAsync(pageData!);
                IHtmlCollection<IElement> tags = doc.QuerySelectorAll(".ui.celled.list.releases-download-list");
                foreach (var tag in tags)
                {
                    var targetUrl = (tag?.FirstElementChild?.FirstElementChild as IHtmlAnchorElement)?.PathName ?? string.Empty;
                    if (targetUrl.Contains(version) && targetUrl.Contains(".exe"))
                    {
                        result = "https://gitee.com" + targetUrl;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
