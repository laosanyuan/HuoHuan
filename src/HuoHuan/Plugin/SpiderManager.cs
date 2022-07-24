using HuoHuan.Core.Spider;
using HuoHuan.Data.DataBase;
using HuoHuan.Glue;
using HuoHuan.Glue.Utils;
using HuoHuan.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HuoHuan.Plugin
{
    internal class SpiderManager
    {
        private GroupFilter _filter = new GroupFilter();
        private GroupDB _db = new GroupDB();
        private IList<IPlugin> _cachePlugins;


        #region [Public Methods]
        public async Task Start(IList<IPlugin> plugins)
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

        #region [Private Methods]

        private void Spider_Crawled(object sender, Glue.CrawlEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Spider_ProgressStatusChanged(object sender, ProgressEventArgs e)
        {
            if (sender is null)
            {
                return;
            }

            ISpider spider = sender as ISpider;
            if (e.Status == SpiderStatus.Stopped || e.Status == SpiderStatus.Finished)
            {
                spider.ProgressStatusChanged -= Spider_ProgressStatusChanged;
                spider.Crawled -= Spider_Crawled;
            }
        }
        #endregion
    }
}
