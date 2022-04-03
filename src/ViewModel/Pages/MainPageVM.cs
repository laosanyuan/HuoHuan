using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HuoHuan.Core;
using HuoHuan.Core.Spider;
using HuoHuan.Data;
using HuoHuan.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HuoHuan.ViewModel.Pages
{
    public class MainPageVM : ViewModelBase
    {
        private readonly ISpider spider;

        public MainPageVM()
        {
            this.spider = MainController.Instance.Spider;
        }

        #region [Properties]
        private CrawlInfo crawl;
        /// <summary>
        /// 爬取数据
        /// </summary>
        public CrawlInfo Crawl
        {
            get => crawl;
            set
            {
                crawl = value;
                RaisePropertyChanged(nameof(Crawl), value);
            }
        }
        private DownloadInfo download;
        /// <summary>
        /// 下载数据
        /// </summary>
        public DownloadInfo Download
        {
            get => download;
            set
            {
                download = value;
                RaisePropertyChanged(nameof(Download), value);
            }
        }

        private ObservableCollection<DisplayImageInfo> ursl;
        /// <summary>
        /// 随机图片链接列表
        /// </summary>
        public ObservableCollection<DisplayImageInfo> Urls
        {
            get => ursl;
            set
            {
                ursl = value;
                RaisePropertyChanged(nameof(Urls), value);
            }
        }

        private DisplayImageInfo test = new DisplayImageInfo() { Url = "https://common.cnblogs.com/images/logo/logo20170227.png" };

        public DisplayImageInfo Test
        {
            get => test;
            set
            {
                test = value;
                RaisePropertyChanged(nameof(Test), value);
            }
        }

        public string TestStr { get; set; } = "https://common.cnblogs.com/images/logo/logo20170227.png";


        private string staus;
        /// <summary>
        /// 运行状态
        /// </summary>
        public string Staus
        {
            get => staus;
            set
            {
                staus = value;
                RaisePropertyChanged(nameof(Staus), value);
            }
        }
        #endregion

        #region [Commands]
        /// <summary>
        /// 开始爬取
        /// </summary>
        public ICommand StartCrawlCommand => new Lazy<RelayCommand<bool>>(() => new RelayCommand<bool>(isStart =>
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
        })).Value;
        /// <summary>
        /// 暂停
        /// </summary>
        public ICommand PauseCrawlCommand => new Lazy<RelayCommand<bool>>(() => new RelayCommand<bool>(isPause =>
        {
            this.spider?.PasueCrawl(isPause);
        })).Value;
        /// <summary>
        /// 开始下载
        /// </summary>
        public ICommand StartDownloadCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            Task.Run(() =>
            {
                this.spider?.StartDownload(arg =>
                {
                    this.Download = new DownloadInfo() { Url = arg.GroupData.SourceUrl, Count = arg.DownloadedCount, LaveCount = arg.LaveCount };
                });
            });
        })).Value;
        /// <summary>
        /// 停止下载
        /// </summary>
        public ICommand StopDownloadCommand => new Lazy<RelayCommand>(() => new RelayCommand(() => this.spider?.StopDownload())).Value;
        #endregion
    }
}
