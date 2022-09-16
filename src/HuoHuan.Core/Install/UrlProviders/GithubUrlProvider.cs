using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace HuoHuan.Core.Install.UrlProviders
{
    public class GithubUrlProvider : IUrlPriovider
    {
        public async Task<string> GetDownloadUrl(string url, string version)
        {
            string result = null!;
            string tmpUrl = url + "/expanded_assets/" + version;
            try
            {
                using HttpClient client = new();
                var response = await client.GetAsync(tmpUrl, HttpCompletionOption.ResponseHeadersRead);
                var pageData = await response.Content.ReadAsStringAsync();

                HtmlParser parser = new();
                IHtmlDocument doc = await parser.ParseDocumentAsync(pageData!);
                IHtmlCollection<IElement> tags = doc.QuerySelectorAll(".Truncate");
                foreach (var tag in tags)
                {
                    var targetUrl = (tag as IHtmlAnchorElement)?.PathName ?? string.Empty;
                    if (targetUrl.Contains(version) && targetUrl.Contains(".exe"))
                    {
                        result = "https://github.com" + targetUrl;
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
