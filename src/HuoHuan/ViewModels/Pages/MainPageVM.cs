using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.Core;
using HuoHuan.Core.Spider;
using HuoHuan.Data;
using HuoHuan.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

#nullable disable

namespace HuoHuan.ViewModels.Pages
{
    public class MainPageVM : ObservableObject
    {
        private readonly ISpider spider;

        #region [Properties]
        private CrawlInfo crawl;
        /// <summary>
        /// 爬取数据
        /// </summary>
        public CrawlInfo Crawl
        {
            get => crawl;
            set => SetProperty(ref crawl, value);
        }
        private DownloadInfo download;
        /// <summary>
        /// 下载数据
        /// </summary>
        public DownloadInfo Download
        {
            get => download;
            set => SetProperty(ref download, value);
        }

        private ObservableCollection<DisplayImageInfo> ursl;
        /// <summary>
        /// 随机图片链接列表
        /// </summary>
        public ObservableCollection<DisplayImageInfo> Urls
        {
            get => ursl;
            set => SetProperty(ref ursl, value);
        }

        private string status;
        /// <summary>
        /// 运行状态
        /// </summary>
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }
        #endregion

        #region [Commands]
        private readonly Lazy<RelayCommand<bool?>> _startCrawlCommand;
        /// <summary>
        /// 开始爬取
        /// </summary>
        public ICommand StartCrawlCommand => _startCrawlCommand.Value;

        private readonly Lazy<RelayCommand<bool?>> _pauseCrawlCommand;
        /// <summary>
        /// 暂停
        /// </summary>
        public ICommand PauseCrawlCommand => _pauseCrawlCommand.Value;

        private readonly Lazy<RelayCommand> _startDownloadCommand;
        /// <summary>
        /// 开始下载
        /// </summary>
        public ICommand StartDownloadCommand => _startDownloadCommand.Value;

        private readonly Lazy<RelayCommand> _stopDownloadCommand;
        /// <summary>
        /// 停止下载
        /// </summary>
        public ICommand StopDownloadCommand => _stopDownloadCommand.Value;
        #endregion

        public MainPageVM()
        {
            this.spider = MainController.Instance.Spider;

            this._startCrawlCommand = new Lazy<RelayCommand<bool?>>(() => new RelayCommand<bool?>(isStart => SwitchCrawl(isStart == true)));
            this._pauseCrawlCommand = new Lazy<RelayCommand<bool?>>(() => new RelayCommand<bool?>(isPause => this.spider?.PasueCrawl(isPause == true)));
            this._startDownloadCommand = new Lazy<RelayCommand>(() => new RelayCommand(() =>
            {
                Task.Run(() =>
                {
                    this.spider?.StartDownload(arg =>
                    {
                        this.Download = new DownloadInfo() { Url = arg.GroupData.SourceUrl, Count = arg.DownloadedCount, LaveCount = arg.LaveCount };
                    });
                });
            }));
            this._stopDownloadCommand = new Lazy<RelayCommand>(() => new RelayCommand(() => this.spider?.StopDownload()));
        }

        private void SwitchCrawl(bool isStart)
        {
            if (isStart)
            {
                Task.Run(() =>
                {
                    this.spider?.StartCrawl(SpiderKey.TiebaDatas, arg =>
                    {
                        // 更新爬取信息
                        this.Crawl = new CrawlInfo() { Url = arg.GroupData?.SourceUrl, CrawledCount = arg.CrawledCount, Process = arg.Process };

                        // 更新图片列表
                        if (!arg.IsFinish)
                        {
                            var url = new DisplayImageInfo() { Url = arg.GroupData?.SourceUrl, IsValid = arg.IsValidImage };

                            if (ursl == null)
                            {
                                Urls = new ObservableCollection<DisplayImageInfo>() { url };
                            }
                            else if (Urls.Count < 12)
                            {
                                Urls.Add(url);
                            }
                            else
                            {
                                // 随机分配至展示部分
                                var index = new Random().Next(12);
                                Urls[index] = url;
                            }
                        }
                    });
                });
            }
            else
            {
                this.spider?.StopCrawl();
            }
        }
    }
}
