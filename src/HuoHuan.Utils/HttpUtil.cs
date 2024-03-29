﻿namespace HuoHuan.Utils
{
    public class HttpUtil
    {
        /// <summary>
        /// 设置伪装请求头
        /// </summary>
        /// <param name="http"></param>
        public static void SetHeaders(HttpClient http, bool isKeepAlive = true)
        {
            http.Timeout = new TimeSpan(0, 0, 2, 0);
            http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            http.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            http.DefaultRequestHeaders.Add("Connection", isKeepAlive ? "keep-alive" : "close");
            http.DefaultRequestHeaders.Add("Referer", "http://www.baidu.com/");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36");
        }
    }
}
