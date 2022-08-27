using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.Enums;
using HuoHuan.Extensions;
using HuoHuan.Models;
using HuoHuan.Plugin;
using HuoHuan.Plugin.Contracs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HuoHuan.ViewModels.Pages
{
    [ObservableObject]
    public partial class HomePageVM
    {
        #region [Fileds]
        private readonly SpiderManager _spiderManager = new();
        private readonly Timer _timer = null!;
        private readonly Random _random = new();
        #endregion

        #region [Dependency Properties]
        /// <summary>
        /// 爬虫集合
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<SpiderInfo> _spiderInfos = null!;

        /// <summary>
        /// 获取成功总数
        /// </summary>
        [ObservableProperty]
        private int _successCount;

        /// <summary>
        /// 展示URL。后续替换为其他形式
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<DisplayImageInfo> _urls = new();

        /// <summary>
        /// 当前是否存在正在运行的爬取器
        /// </summary>
        [ObservableProperty]
        private bool _isRunning;
        #endregion 

        public HomePageVM()
        {
            this._spiderManager.Crawled += SpiderManager_Crawled;
            this._spiderManager.ProgressStatusChanged += SpiderManager_ProgressStatusChanged;

            this._timer = new Timer(_ => this.UpdateShowImages());
            this._timer.Change(Timeout.Infinite, 150);

            this.LoadSpider();
        }

        #region [Commands]
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="spider"></param>
        [RelayCommand]
        private void Start(IPlugin? plugin)
        {
            if (plugin is null)
            {
                Parallel.ForEach(this.SpiderInfos, t => t.Reset());
                this.Urls.Clear();
                this._spiderManager.StartAll();
                this.SuccessCount = 0;
            }
            else
            {
                var tmp = this.SpiderInfos?.FirstOrDefault(t => t?.Plugin == plugin);
                if (tmp is not null)
                {
                    tmp.Reset();
                    this._spiderManager.Start(plugin);
                    // 如果当前已没有正在运行的Spider，则重新计数
                    if (!(this.SpiderInfos?.Any(t => t.Status == SpiderStatus.Running || t.Status == SpiderStatus.Paused) == true))
                    {
                        this.SuccessCount = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="spider"></param>
        [RelayCommand]
        private void Stop(IPlugin? plugin)
        {
            if (plugin is null)
            {
                this._spiderManager.StopAll();
            }
            else
            {
                this._spiderManager.Stop(plugin);
            }
        }
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="spider"></param>
        [RelayCommand]
        private void Pause(IPlugin? plugin)
        {
            if (plugin is null)
            {
                this._spiderManager.PauseAll();
            }
            else
            {
                this._spiderManager.Pause(plugin);
            }
        }
        /// <summary>
        /// 继续
        /// </summary>
        /// <param name="spider"></param>
        [RelayCommand]
        private void Continue(IPlugin? plugin)
        {
            if (plugin is null)
            {
                this._spiderManager.ContinueAll();
            }
            else
            {
                this._spiderManager.Continue(plugin);
            }
        }

        /// <summary>
        /// 操作全部爬取器
        /// </summary>
        /// <param name="param"></param>
        [RelayCommand]
        private void OperationAll(object param)
        {
            if (param is SpiderOperationStatus status)
            {
                switch (status)
                {
                    case SpiderOperationStatus.Start:
                        this.Start(null);
                        this._timer.Change(0, 150);
                        break;
                    case SpiderOperationStatus.Stop:
                        this.Stop(null);
                        this._timer.Change(Timeout.Infinite, 150);
                        break;
                    case SpiderOperationStatus.Pause:
                        this.Pause(null);
                        break;
                    case SpiderOperationStatus.Continue:
                        this.Continue(null);
                        break;
                }
            }
        }
        #endregion

        #region [Private Methods]
        private void UpdateShowImages()
        {
            if (this._spiderManager.ImageChannels.Reader.TryRead(out var reader))
            {
                var image = new DisplayImageInfo()
                {
                    Url = reader.Url,
                    IsValid = reader.IsValidate
                };
                if (this.Urls.Count < 12)
                {
                    this.Urls.Add(image);
                }
                else
                {
                    this.Urls[this._random.Next(11)] = image;
                }
            }
        }

        /// <summary>
        /// 加载爬取器
        /// </summary>
        private void LoadSpider()
        {
            this.SpiderInfos = new ObservableCollection<SpiderInfo>(PluginLoader.Plugins.Select(
                t => new SpiderInfo()
                {
                    Plugin = t,
                    Status = SpiderStatus.Waiting
                }));
        }

        private void SpiderManager_ProgressStatusChanged(object sender, SpiderProgressEventArgs e)
        {
            var spiderInfo = this.SpiderInfos?.FirstOrDefault(t => t?.Plugin?.Spider == e.Spider);
            if (spiderInfo is not null)
            {
                spiderInfo.Progress = e.Progress;
                spiderInfo.Status = e.Status;

                if (this.SpiderInfos?.All(t => t.Status.IsEnded()) == true)
                {
                    this.IsRunning = false;
                }
            }
        }

        private void SpiderManager_Crawled(object sender, SpiderCrawlEventArgs e)
        {
            var spiderInfo = this.SpiderInfos?.FirstOrDefault(t => t?.Plugin?.Spider == e.Spider);
            if (spiderInfo is not null)
            {
                spiderInfo.ImageUrl = e.Url;
                spiderInfo.Count++;
                // 更新总数
                this.SuccessCount = this.SpiderInfos?.Sum(t => t.Count) ?? 0;
            }
        }
        #endregion
    }
}
