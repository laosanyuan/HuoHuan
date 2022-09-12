using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
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
        private void Download()
        {

        }
        /// <summary>
        /// 点赞
        /// </summary>
        [RelayCommand]
        private void Like() => Process.Start(ConfigurationManager.AppSettings["ProjectUrl"]!);
        /// <summary>
        /// 手动下载
        /// </summary>
        [RelayCommand]
        private void ManualDownload() => Process.Start(ConfigurationManager.AppSettings["DownloadUrl"]!);
        #endregion
    }
}
