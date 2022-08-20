using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.Models;
using HuoHuan.Plugin;
using HuoHuan.Plugin.Contracs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HuoHuan.ViewModels.Pages
{
    [ObservableObject]
    public partial class HomePageVM
    {
        #region [Fileds]
        private readonly SpiderManager _spiderManager = new();
        private Dictionary<ISpider, int> _cacheCount = new();  // 爬取数量缓存
        #endregion

        #region [Properties]

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
        }

        #region [Commands]
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="spider"></param>
        [ICommand]
        private void Start(ISpider? spider)
        {
            if (spider is null)
            {
                this.LoadSpider();
                Parallel.ForEach(this.SpiderInfos, (spiderInfo) => spiderInfo.Spider.Start());
                this._cacheCount.Clear();
                this.SuccessCount = 0;
            }
            else
            {
                var tmp = this.SpiderInfos?.FirstOrDefault(t => t.Spider == spider);
                if (tmp is not null)
                {
                    // 如果当前已没有正在运行的Spider，则重新计数
                    if (!(this.SpiderInfos?.Any(t => t.Status == SpiderStatus.Running || t.Status == SpiderStatus.Paused) == true))
                    {
                        this._cacheCount.Clear();
                        this.SuccessCount = 0;
                    }
                    tmp?.Spider.Start();
                }
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="spider"></param>
        [ICommand]
        private void Stop(ISpider? spider)
        {
            if (spider is null)
            {
                Parallel.ForEach(this.SpiderInfos,
                    info =>
                    {
                        if (info.Status == SpiderStatus.Running || info.Status == SpiderStatus.Paused)
                        {
                            info.Spider.Stop();
                        }
                    });
            }
            else
            {
                var tmp = this.SpiderInfos?.FirstOrDefault(t => t.Spider == spider);
                if (tmp is not null)
                {
                    if (tmp.Status == SpiderStatus.Running || tmp.Status == SpiderStatus.Paused)
                    {
                        tmp?.Spider.Stop();
                    }
                }
            }
        }
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="spider"></param>
        [ICommand]
        private void Pause(ISpider? spider)
        {
            if (spider is null)
            {
                Parallel.ForEach(this.SpiderInfos,
                    info =>
                    {
                        if (info.Status == SpiderStatus.Running)
                        {
                            info.Spider.Pause();
                        }
                    });
            }
            else
            {
                var tmp = this.SpiderInfos?.FirstOrDefault(t => t.Spider == spider);
                if (tmp is not null && tmp.Status == SpiderStatus.Running)
                {
                    tmp?.Spider.Pause();
                }
            }
        }
        /// <summary>
        /// 继续
        /// </summary>
        /// <param name="spider"></param>
        [ICommand]
        private void Continue(ISpider? spider)
        {
            if (spider is null)
            {
                Parallel.ForEach(this.SpiderInfos,
                    info =>
                    {
                        if (info.Status == SpiderStatus.Paused)
                        {
                            info.Spider.Continue();
                        }
                    });
            }
            else
            {
                var tmp = this.SpiderInfos?.FirstOrDefault(t => t.Spider == spider);
                if (tmp is not null && tmp.Status == SpiderStatus.Paused)
                {
                    tmp?.Spider.Continue();
                }
            }
        }
        #endregion

        #region [Private Commands]
        private void LoadSpider()
        {
            this._spiderInfos = new ObservableCollection<SpiderInfo>(PluginLoader.Plugins.Select(
                t => new SpiderInfo()
                {
                    Name = t.Name,
                    Spider = t.Spider,
                    Status = SpiderStatus.Waiting
                }));
        }

        private void SpiderManager_ProgressStatusChanged(object sender, SpiderProgressEventArgs e)
        {
            var spiderInfo = this.SpiderInfos?.FirstOrDefault(t => t.Spider == e.Spider);
            if (spiderInfo is not null)
            {
                spiderInfo.Progress = e.Progress;
                spiderInfo.Count = e.Count;
                spiderInfo.Status = e.Status;

                // 更新总数
                if (this._cacheCount.ContainsKey(e.Spider))
                {
                    this._cacheCount[e.Spider] = e.Count;
                }
                else
                {
                    this._cacheCount[e.Spider] = e.Count;
                }
                this.SuccessCount = this._cacheCount.Sum(t => t.Value);
            }
        }

        private void SpiderManager_Crawled(object sender, SpiderCrawlEventArgs e)
        {
            var spiderInfo = this.SpiderInfos?.FirstOrDefault(t => t.Spider == e.Spider);
            if (spiderInfo is not null)
            {
                spiderInfo.ImageUrl = e.Url;
            }
        }
        #endregion
    }
}
