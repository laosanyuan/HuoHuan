using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Configuration;

namespace HuoHuan.Core.Install.UrlProviders
{
    public class GiteeUrlProvider : IUrlProvider
    {
        public async Task<string> GetDownloadUrl(string version)
        {
            string result = null!;
            var url = ConfigurationManager.AppSettings["GiteeDownloadUrl"];
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
