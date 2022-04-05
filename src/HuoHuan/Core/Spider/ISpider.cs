using HuoHuan.Enums;
using HuoHuan.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuoHuan.Core.Spider
{
    internal interface ISpider
    {
        /// <summary>
        /// 设置过滤器
        /// </summary>
        /// <param name="path"></param>
        /// <param name="types"></param>
        void SetFilter(string path, params QRCodeType[] types);

        /// <summary>
        /// 开始爬取
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="callback"></param>
        Task StartCrawl(List<SpiderData> keys, Action<CrawlEventArgs> callback);
        /// <summary>
        /// 停止爬取
        /// </summary>
        void StopCrawl();
        /// <summary>
        /// 暂停爬取
        /// </summary>
        /// <param name="isPause">暂停或恢复</param>
        void PasueCrawl(bool isPause);

        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="callback"></param>
        Task StartDownload(Action<DownloadEventArgs> callback);
        /// <summary>
        /// 停止下载
        /// </summary>
        void StopDownload();
    }
}
