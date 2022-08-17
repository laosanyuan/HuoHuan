using HuoHuan.Data.DataBase;
using HuoHuan.Models;
using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HuoHuan.Plugin
{
    internal delegate void SpiderProgressEventHandler(object sender, SpiderProgressEventArgs e);
    internal delegate void SpiderCrawledEventHandler(object sender, SpiderCrawlEventArgs e);

    internal class SpiderManager
    {
        #region [Fileds]
        private GroupFilter _filter = new GroupFilter();
        private GroupDB _db = new GroupDB();
        private IList<IPlugin> _cachePlugins = null!;
        #endregion

        #region [Properties]
        public Channel<string> ImageChannels;
        #endregion

        #region [Events]
        /// <summary>
        /// 进度状态变更
        /// </summary>
        public event SpiderProgressEventHandler ProgressStatusChanged = null!;
        /// <summary>
        /// 爬取结果通知
        /// </summary>
        public event SpiderCrawledEventHandler Crawled = null!;
        #endregion

        #region [Public Methods]
        public void Start(IList<IPlugin> plugins)
        {
            this._cachePlugins = plugins;
            Parallel.ForEach(plugins, t =>
            {
                if (t.IsValid())
                {
                    t.Spider.ProgressStatusChanged -= Spider_ProgressStatusChanged;
                    t.Spider.Crawled -= Spider_Crawled;

                    t.Spider.ProgressStatusChanged += Spider_ProgressStatusChanged;
                    t.Spider.Crawled += Spider_Crawled;
                    t.Init();
                    t.Spider.Start();
                }
            });
        }

        public void Continue(IPlugin plugin = null!)
        {
            if (plugin is null)
            {
                Parallel.ForEach(this._cachePlugins, t =>
                {
                    t.Spider.Continue();
                });
            }
            else
            {
                this._cachePlugins.FirstOrDefault(t => t == plugin)?.Spider.Continue();
            }
        }

        public void Pause(IPlugin plugin = null!)
        {
            if (plugin is null)
            {
                Parallel.ForEach(this._cachePlugins, t =>
                {
                    t.Spider.Pause();
                });
            }
            else
            {
                this._cachePlugins.FirstOrDefault(t => t == plugin)?.Spider.Pause();
            }
        }

        public void Stop(IPlugin plugin = null!)
        {
            if (plugin is null)
            {
                Parallel.ForEach(this._cachePlugins, t =>
                {
                    t.Spider.Stop();
                });
            }
            else
            {
                this._cachePlugins.FirstOrDefault(t => t == plugin)?.Spider.Stop();
            }
        }

        /// <summary>
        /// 保存群数据
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Save(GroupData group)
        {
            var fileName = Path.Combine(FolderUtil.ImagesFolder, group.FileName);
            await ImageUtil.SaveImageFile(group.SourceUrl, fileName);
            group.LocalPath = FolderUtil.ImagesFolder;
            await this._db.InsertGroup(group);
        }
        #endregion

        public SpiderManager()
        {
            this.ImageChannels = Channel.CreateBounded<string>(
                new BoundedChannelOptions(30)
                {
                    FullMode = BoundedChannelFullMode.DropOldest
                });
        }

        #region [Private Methods]

        private async void Spider_Crawled(object sender, CrawlEventArgs e)
        {
            if (sender is not null)
            {
                var result = await this._filter.IsValidImage(e.Url);
                if (result.Item1)
                {
                    var group = this._filter.GetGroupData(e.Url, result.Item2);
                    await this.Save(group);
                    this.Crawled?.Invoke(this, new SpiderCrawlEventArgs(e, (sender as ISpider)!, group));
                }
            }
            await this.ImageChannels.Writer.WriteAsync(e.Url);
        }

        private void Spider_ProgressStatusChanged(object sender, ProgressEventArgs e)
        {
            if (sender is null)
            {
                return;
            }

            ISpider spider = (sender as ISpider)!;
            if (e.Status == SpiderStatus.Stopped || e.Status == SpiderStatus.Finished)
            {
                spider!.ProgressStatusChanged -= Spider_ProgressStatusChanged;
                spider.Crawled -= Spider_Crawled;
            }

            this.ProgressStatusChanged?.Invoke(this, new SpiderProgressEventArgs(e, spider!));
        }
        #endregion
    }

    internal class SpiderProgressEventArgs : ProgressEventArgs
    {
        public SpiderProgressEventArgs(ProgressEventArgs args, ISpider spider)
        {
            base.Progress = args.Progress;
            base.Count = args.Count;
            base.LeftTime = args.LeftTime;
            base.UsedTime = args.UsedTime;
            base.Status = args.Status;
            this.Spider = spider;
        }

        public ISpider Spider { get; init; } = null!;
    }

    internal class SpiderCrawlEventArgs : CrawlEventArgs
    {
        public SpiderCrawlEventArgs(CrawlEventArgs args, ISpider spider, GroupData group)
        {
            base.Url = args.Url;
            this.Spider = spider;
            this.Group = group;
        }
        public ISpider Spider { get; init; } = null!;
        public GroupData Group { get; init; } = null!;
    }
}
