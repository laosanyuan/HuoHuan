using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.Enums;
using HuoHuan.Models;
using HuoHuan.Plugin;
using HuoHuan.Plugin.Contracs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HuoHuan.ViewModels.Pages
{
    [ObservableObject]
    public partial class HomePageVM
    {
        #region [Fileds]
        private readonly SpiderManager _spiderManager = new();
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
        #endregion 

        public HomePageVM()
        {
            this._spiderManager.Crawled += SpiderManager_Crawled;
            this._spiderManager.ProgressStatusChanged += SpiderManager_ProgressStatusChanged;

            this.LoadSpider();
        }

        #region [Commands]
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="spider"></param>
        [ICommand]
        private void Start(IPlugin? plugin)
        {
            if (plugin is null)
            {
                this._spiderManager.StartAll();
                this.SuccessCount = 0;
            }
            else
            {
                var tmp = this.SpiderInfos?.FirstOrDefault(t => t?.Plugin == plugin);
                if (tmp is not null)
                {
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
        [ICommand]
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
        [ICommand]
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
        [ICommand]
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
        [ICommand]
        private void OperationAll(object param)
        {
            if (param is SpiderOperationStatus status)
            {
                switch (status)
                {
                    case SpiderOperationStatus.Start:
                        this.Start(null);
                        break;
                    case SpiderOperationStatus.Stop:
                        this.Stop(null);
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

        #region [Private Commands]
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
                spiderInfo.Count = e.Count;
                spiderInfo.Status = e.Status;

                // 更新总数
                this.SuccessCount = this.SpiderInfos?.Sum(t => t.Count) ?? 0;
            }
        }

        private void SpiderManager_Crawled(object sender, SpiderCrawlEventArgs e)
        {
            var spiderInfo = this.SpiderInfos?.FirstOrDefault(t => t?.Plugin?.Spider == e.Spider);
            if (spiderInfo is not null)
            {
                spiderInfo.ImageUrl = e.Url;
            }
        }
        #endregion
    }
}
