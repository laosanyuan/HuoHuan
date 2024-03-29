﻿using HuoHuan.DataBase.Models;
using HuoHuan.DataBase.Services;
using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;
using System.Drawing;
using System.Threading.Channels;

namespace HuoHuan.Core.Plugin
{
    public delegate void SpiderProgressEventHandler(object sender, SpiderProgressEventArgs e);
    public delegate void SpiderCrawledEventHandler(object sender, SpiderCrawlEventArgs e);

    public class SpiderManager
    {
        #region [Fileds]
        private readonly GroupFilter _filter = new();   // 群图片过滤器
        private readonly GroupDB _db = new();           // 群数据库
        private QRCodeGenerater _qrCodeGenerater;
        #endregion

        #region [Properties]
        /// <summary>
        /// 展示用图片队列
        /// </summary>
        public Channel<(Bitmap Bitmap, bool IsValidate)> ImageChannels { get; private set; }
        /// <summary>
        /// 生成图片字体
        /// </summary>
        public FontFamily FontFamily { set => this._qrCodeGenerater.FontFamily = value; }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        public SpiderManager(string logo = null!)
        {
            this.ImageChannels = Channel.CreateBounded<(Bitmap, bool)>(
                new BoundedChannelOptions(50)
                {
                    FullMode = BoundedChannelFullMode.DropOldest
                });
            this._qrCodeGenerater = new QRCodeGenerater(FolderUtil.ImagesFolder, logo);
        }

        #region [Public Methods]
        /// <summary>
        /// 开始全部
        /// </summary>
        public void StartAll() => Parallel.ForEach(PluginLoader.Plugins, t => Start(t));
        /// <summary>
        /// 全部开始
        /// </summary>
        /// <param name="plugin"></param>
        public async void Start(IPlugin plugin)
        {
            if (plugin?.IsValid() == true)
            {
                plugin.Spider.ProgressStatusChanged -= Spider_ProgressStatusChanged;
                plugin.Spider.Crawled -= Spider_Crawled;

                plugin.Spider.ProgressStatusChanged += Spider_ProgressStatusChanged;
                plugin.Spider.Crawled += Spider_Crawled;
                await plugin.Init();
                plugin.Spider.Start();
            }
        }
        /// <summary>
        /// 继续全部
        /// </summary>
        public void ContinueAll() => Parallel.ForEach(PluginLoader.Plugins, t => Continue(t));
        /// <summary>
        /// 继续
        /// </summary>
        /// <param name="plugin"></param>
        public void Continue(IPlugin plugin = null!) => plugin?.Spider?.Continue();
        /// <summary>
        /// 暂停全部
        /// </summary>
        public void PauseAll() => Parallel.ForEach(PluginLoader.Plugins, t => Pause(t));
        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="plugin"></param>
        public void Pause(IPlugin plugin = null!) => plugin?.Spider?.Pause();
        /// <summary>
        /// 停止全部
        /// </summary>
        public void StopAll() => Parallel.ForEach(PluginLoader.Plugins, t => Stop(t));
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="plugin"></param>
        public void Stop(IPlugin plugin)
        {
            if (plugin is not null)
            {
                plugin.Spider.Stop();
                plugin.Spider.ProgressStatusChanged -= Spider_ProgressStatusChanged;
                plugin.Spider.Crawled -= Spider_Crawled;
            }
        }
        #endregion

        #region [Private Methods]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
        private async void Spider_Crawled(object sender, CrawlEventArgs e)
        {
            if (sender is not null)
            {
                var image = await ImageUtil.GetBitmapFromUrl(e.Url);
                if (e.NeedFilter)
                {
                    if (image is not null)
                    {
                        var (IsValidate, Message, Bitmap) = await this._filter.IsValidImage(image, e.Url);
                        if (IsValidate)
                        {
                            var group = this._filter.GetGroupData(Bitmap, Message, e.Url);
                            if (group is not null)
                            {
                                await this.Save(group);
                                this.Crawled?.Invoke(this, new SpiderCrawlEventArgs(e, (sender as ISpider)!, group));
                            }
                        }
                        await this.ImageChannels.Writer.WriteAsync((image, IsValidate));
                    }
                }
                else
                {
                    if (!await this._filter.IsRepeatImage(e.Url)
                        && ImageUtil.IsQRCode(image, out string content))
                    {
                        var group = new GroupImage()
                        {
                            GroupName = e.Name,
                            InvalidateDate = e.InvalidTime,
                            Url = e.Url,
                            FileName = e.Url.Split("/").LastOrDefault()?.Replace(".jpg", "") + ".jpg",
                            QRText = content,
                        };
                        await this.Save(group);
                        this.Crawled?.Invoke(this, new SpiderCrawlEventArgs(e, (sender as ISpider)!, group));
                    }
                    await this.ImageChannels.Writer.WriteAsync((image, true));
                }
            }
        }

        private void Spider_ProgressStatusChanged(object sender, ProgressEventArgs e)
        {
            if (sender is not null)
            {
                ISpider spider = (sender as ISpider)!;
                if (e.Status == SpiderStatus.Stopped || e.Status == SpiderStatus.Finished)
                {
                    spider!.ProgressStatusChanged -= Spider_ProgressStatusChanged;
                    spider.Crawled -= Spider_Crawled;
                }
                this.ProgressStatusChanged?.Invoke(this, new SpiderProgressEventArgs(e, spider!));
            }
        }

        /// <summary>
        /// 保存群数据
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private async Task Save(GroupImage group)
        {
            var fileName = Path.Combine(FolderUtil.ImagesFolder, group.FileName);
            if (this._qrCodeGenerater.GenerateImage(group, fileName))
            {
                group.LocalPath = FolderUtil.ImagesFolder;
                await this._db.InsertGroup(group);
            }
        }
        #endregion
    }

    public class SpiderProgressEventArgs : ProgressEventArgs
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

    public class SpiderCrawlEventArgs : CrawlEventArgs
    {
        public SpiderCrawlEventArgs(CrawlEventArgs args, ISpider spider, GroupImage group)
        {
            base.Url = args.Url;
            this.Spider = spider;
            this.Group = group;
        }
        public ISpider Spider { get; init; } = null!;
        public GroupImage Group { get; init; } = null!;
    }
}
