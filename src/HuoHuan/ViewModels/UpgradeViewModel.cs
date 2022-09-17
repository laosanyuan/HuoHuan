using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HuoHuan.Core;
using HuoHuan.Core.Install;
using HuoHuan.Core.Install.UrlProviders;
using HuoHuan.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static HuoHuan.Views.MainWindow;

namespace HuoHuan.ViewModels
{
    [ObservableObject]
    public partial class UpgradeViewModel
    {
        #region [Dependency Properties]
        /// <summary>
        /// 新版本
        /// </summary>
        [ObservableProperty]
        private string _newVersion = null!;
        /// <summary>
        /// 升级信息
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _messages = null!;
        /// <summary>
        /// 升级标题
        /// </summary>
        [ObservableProperty]
        private string _title = null!;
        /// <summary>
        /// 升级中
        /// </summary>
        [ObservableProperty]
        private bool _isUpgrading = false;
        /// <summary>
        /// 升级进度
        /// </summary>
        [ObservableProperty]
        private int _progressValue;
        #endregion

        public UpgradeViewModel()
        {
            WeakReferenceMessenger.Default.Register<VersionInfo, string>(this, "UpgradeVersion", (vm, info) =>
            {
                this.Title = info.Title;
                this.NewVersion = info.Version;
                this.Messages = new ObservableCollection<string>(info.Messages);
            });
        }

        #region [Commands]
        /// <summary>
        /// 下载并升级
        /// </summary>
        [RelayCommand]
        private async void Download()
        {
            this.IsUpgrading = true;
            var result = await DownloadInstall();
            if (result)
            {
                // 关闭本进程
                Environment.Exit(0);
            }
        }
        /// <summary>
        /// 手动下载
        /// </summary>
        [RelayCommand]
        private void ManualDownload() => WebUtil.OpenUrl(LocalConfigManager.DownloadInstallUrl);
        #endregion

        /// <summary>
        /// 下载安装包并启动
        /// </summary>
        /// <returns></returns>
        private async Task<bool> DownloadInstall()
        {
            var provider = UrlProvider.Instance;
            var url = await provider.GetDownloadUrl(this.NewVersion);

            var downloader = new InstallDownloader();
            downloader.DownloadProgressChanged += (_, e) =>
            {
                this.ProgressValue = (int)(e.DownloadedSize * 100.0 / e.AllSize);
            };
            var fileName = Path.Combine(FolderUtil.TmpPath, $"setup - {this.NewVersion}.exe");
            await downloader.DownloadAsync(url, fileName, new System.Threading.CancellationToken());

            if (this.ProgressValue == 100)
            {

                ProcessStartInfo info = new ProcessStartInfo();
                info.WorkingDirectory = FolderUtil.TmpPath;
                info.FileName = fileName;
                info.Arguments = "";
                info.Verb = "runas";
                Process.Start(info);
                return true;
            }
            return false;
        }
    }
}
