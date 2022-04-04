using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HuoHuan.Core.Filter;
using HuoHuan.Enums;
using HuoHuan.Models;
using HuoHuan.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HuoHuan.Core.Spider
{
    internal class TiebaSpider : ISpider
    {
        private readonly List<IGroupFilter> filters = new();
        private readonly HtmlParser parser = new();
        private readonly Queue<GroupData> GroupDatas = new();
        private int downloadedCount = 0;    // 已下载数量
        private int crawledCount = 0;       // 已获取数量

        public bool IsCrawling { get; private set; }
        public bool IsPauseCrawl { get; set; }
        public bool IsDownloading { get; private set; }
        public string Path { get; protected set; } = null!;

        public void SetFilter(string path, params QRCodeType[] types)
        {
            this.Path = path;
            this.filters.Clear();
            foreach (var type in types)
            {
                var result = GroupFilterFactory.CreatFilter(type);
                if (result != null)
                {
                    filters.Add(result);
                }
            }
        }

        public async Task StartCrawl(List<SpiderData> keys, Action<CrawlEventArgs> callback)
        {
            if (String.IsNullOrEmpty(this.Path) || this.filters.Count == 0)
            {
                return;
            }

            this.IsCrawling = true;
            this.IsPauseCrawl = false;
            this.crawledCount = 0;
            double process = 0.0;

            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < keys[i].Page; j++)
                {
                    HashSet<string> urls = new HashSet<string>();
                    int index = keys[i].Page * 50;
                    try
                    {
                        WebClient client = new WebClient();
                        HttpUtil.SetHeaders(client);
                        string pageData = client.DownloadString($"https://tieba.baidu.com/f?kw={keys[i].Key}&ie=utf-8&pn={index}");
                        IHtmlDocument doc = await this.parser.ParseDocumentAsync(pageData);
                        IHtmlCollection<IElement> tags = doc.QuerySelectorAll(".t_con.cleafix");
                        foreach (IElement tag in tags)
                        {
                            IHtmlCollection<IElement> images = tag.QuerySelectorAll(".threadlist_pic.j_m_pic");
                            for (int k = 0; k < images.Length; k++)
                            {
                                IElement image = images[k];
                                var url = image.GetAttribute("bpic");

                                foreach (var filter in this.filters)
                                {
                                    if (!this.IsCrawling)
                                    {
                                        // 停止爬取
                                        return;
                                    }
                                    while (this.IsPauseCrawl)
                                    {
                                        // 暂停爬取
                                        Thread.Sleep(100);
                                        if (!this.IsCrawling)
                                        {
                                            return;
                                        }
                                    }
                                    if (await filter.IsValidImage(url))
                                    {
                                        var data = filter.GetGroupData(url);

                                        if (data != null)
                                        {
                                            data.Type = filter.FilterType;
                                            this.GroupDatas.Enqueue(data);
                                            filter?.SaveData(data);
                                            this.crawledCount++;
                                        }
                                        else
                                        {
                                            // 后期判断是否保留
                                            data = new GroupData() { SourceUrl = url };
                                        }

                                        try
                                        {
                                            // 估算进度
                                            process = (float)(((i * 1.0 / keys.Count)
                                                    + (j * 1.0 / keys[i].Page / keys.Count)
                                                    + ((k + 1.0) / images.Length / keys[i].Page / keys.Count)) * 100);
                                            callback?.Invoke(new CrawlEventArgs()
                                            {
                                                GroupData = data,
                                                CrawledCount = this.crawledCount,
                                                Process = process,
                                                IsValidImage = true
                                            });
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        callback?.Invoke(new CrawlEventArgs()
                                        {
                                            GroupData = new GroupData { SourceUrl = url },
                                            CrawledCount = this.crawledCount,
                                            Process = process,
                                            IsValidImage = false
                                        });
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            callback?.Invoke(new CrawlEventArgs()
            {
                CrawledCount = this.crawledCount,
                Process = 100,
                IsFinish = true
            });
        }

        public async Task StartDownload(Action<DownloadEventArgs> callback)
        {
            this.IsDownloading = true;
            this.downloadedCount = 0;

            await Task.Factory.StartNew(() =>
            {
                try
                {
                    while (this.GroupDatas.Count > 0)
                    {
                        if (!this.IsDownloading)
                        {
                            return;
                        }
                        var data = this.GroupDatas.Dequeue();

                        var localPath = $"{this.Path}\\{data.FileName}";
                        ImageUtil.SaveImageFile(data.SourceUrl, localPath);
                        data.LocalPath = localPath;
                        this.downloadedCount++;

                        var filter = this.filters.FirstOrDefault(t => t.FilterType == data.Type);
                        filter?.SaveData(data);

                        callback?.Invoke(new DownloadEventArgs() { GroupData = data, LaveCount = this.GroupDatas.Count, DownloadedCount = this.downloadedCount });
                    }
                }
                catch (Exception ex)
                {
                    this.IsDownloading = false;
                }
            });
        }

        public void StopCrawl()
        {
            this.IsCrawling = false;
            this.IsPauseCrawl = false;
        }

        public void StopDownload()
        {
            this.IsDownloading = false;
        }

        public void PasueCrawl(bool isPause)
        {
            if (this.IsCrawling)
            {
                this.IsPauseCrawl = isPause;
            }
        }
    }
}
