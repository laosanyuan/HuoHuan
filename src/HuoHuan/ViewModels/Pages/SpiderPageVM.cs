using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.Glue;
using HuoHuan.Models;
using HuoHuan.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HuoHuan.ViewModels.Pages
{
    public class SpiderPageVM : ObservableObject
    {
        #region [Fileds]
        private readonly SpiderManager _spiderManager = new SpiderManager();
        private Dictionary<ISpider, int> _cacheCount = new Dictionary<ISpider, int>();  // 爬取数量缓存
        #endregion

        #region [Properties]
        private ObservableCollection<SpiderInfo> _spiderInfos = null!;
        public ObservableCollection<SpiderInfo> SpiderInfos
        {
            get => this._spiderInfos;
            set => SetProperty(ref _spiderInfos, value);
        }

        private int _successCount;
        /// <summary>
        /// 获取成功总数
        /// </summary>
        public int SuccessCount
        {
            get => this._successCount;
            set => SetProperty(ref this._successCount, value);
        }
        #endregion 

        #region [Commands]
        private readonly Lazy<RelayCommand<ISpider>> _startCommand;
        /// <summary>
        /// 开始
        /// </summary>
        public ICommand StartCommand => _startCommand.Value;

        private readonly Lazy<RelayCommand<ISpider>> _stopCommand;
        /// <summary>
        /// 停止
        /// </summary>
        public ICommand StopCommand => _stopCommand.Value;

        private readonly Lazy<RelayCommand<ISpider>> _pauseCommand;
        /// <summary>
        /// 暂停
        /// </summary>
        public ICommand PauseCommand => _pauseCommand.Value;

        private readonly Lazy<RelayCommand<ISpider>> _continueCommand;
        /// <summary>
        /// 继续
        /// </summary>
        public ICommand ContinueCommand => _continueCommand.Value;
        #endregion

        public SpiderPageVM()
        {
            this._startCommand = new Lazy<RelayCommand<ISpider>>(() => new RelayCommand<ISpider>(StartCommandExecute));
            this._stopCommand = new Lazy<RelayCommand<ISpider>>(() => new RelayCommand<ISpider>(StopCommandExecute));
            this._pauseCommand = new Lazy<RelayCommand<ISpider>>(() => new RelayCommand<ISpider>(PauseCommandExecute));
            this._continueCommand = new Lazy<RelayCommand<ISpider>>(() => new RelayCommand<ISpider>(ContinueCommandExecute));

            this._spiderManager.Crawled += _spiderManager_Crawled;
            this._spiderManager.ProgressStatusChanged += _spiderManager_ProgressStatusChanged;
        }


        #region [Command Methods]
        private void StartCommandExecute(ISpider? spider)
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

        private void StopCommandExecute(ISpider? spider)
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

        private void PauseCommandExecute(ISpider? spider)
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

        private void ContinueCommandExecute(ISpider? spider)
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

        private void _spiderManager_ProgressStatusChanged(object sender, SpiderProgressEventArgs e)
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

        private void _spiderManager_Crawled(object sender, SpiderCrawlEventArgs e)
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
