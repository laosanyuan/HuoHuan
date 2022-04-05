using System.Net;
using System.Net.Http;

namespace HuoHuan.Utils
{
    internal class HttpUtil
    {
        /// <summary>
        /// 设置伪装请求头
        /// </summary>
        /// <param name="http"></param>
        internal static void SetHeaders(HttpClient http)
        {
            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("Referer", "http://www.baidu.com/");
            //http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.104 Safari/537.36 Core/1.53.4882.400 QQBrowser/9.7.13059.400");

        }

        /// <summary>
        /// 设置伪装请求头
        /// </summary>
        /// <param name="client"></param>
        internal static void SetHeaders(WebClient client)
        {
            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.Headers.Add("Cache-Control", "max-age=0");
            client.Headers.Add(HttpRequestHeader.KeepAlive, "TRUE");
            client.Headers.Add("Referer", "http://www.baidu.com/");
            //client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.104 Safari/537.36 Core/1.53.4882.400 QQBrowser/9.7.13059.400");
        }
    }
}
