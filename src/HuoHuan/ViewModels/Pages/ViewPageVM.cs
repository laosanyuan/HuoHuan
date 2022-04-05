using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.Data;
using HuoHuan.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable

namespace HuoHuan.ViewModels.Pages
{
    public class ViewPageVM : ObservableObject
    {
        #region [Fields]
        private List<string> urls = null!;
        private readonly GroupDB db;    // 微信群链接数据库
        #endregion

        #region [Properties]
        private string displayUrl = SoftwareInfo.LogoPath;
        /// <summary>
        /// 显示Url
        /// </summary>
        public string DisplayUrl
        {
            get => displayUrl;
            set => SetProperty(ref displayUrl, value);
        }

        private int displayIndex;
        /// <summary>
        /// 显示图片索引
        /// </summary>
        public int DisplayIndex
        {
            get => displayIndex;
            set
            {
                if (value == this.displayIndex && !displayUrl.Equals(SoftwareInfo.LogoPath))
                {
                    return;
                }
                if (value < 0 || this.count == 0)
                {
                    value = 0;
                }
                else if (value > urls.Count - 1)
                {
                    value = urls.Count - 1;
                }
                this.DisplayUrl = this.count == 0 ? SoftwareInfo.LogoPath : this.urls[value];

                SetProperty(ref displayIndex, value);
            }
        }

        private int count;
        /// <summary>
        /// 图片数量
        /// </summary>
        public int Count
        {
            get => count;
            set => SetProperty<int>(ref count, value);
        }
        #endregion

        #region [Commands]
        private Lazy<RelayCommand> _refreshDataCommand;
        /// <summary>
        /// 刷新数据
        /// </summary>
        public ICommand RefreshDataCommand => _refreshDataCommand.Value;

        private Lazy<RelayCommand> _previousCommand;
        /// <summary>
        /// 前一张
        /// </summary>
        public ICommand PreviousCommand => _previousCommand.Value;

        private Lazy<RelayCommand> _nextCommand;
        /// <summary>
        /// 后一张
        /// </summary>
        public ICommand NextCommand => _nextCommand.Value;
        #endregion

        public ViewPageVM()
        {
            this.db = new GroupDB("wechat_group");

            this._refreshDataCommand = new Lazy<RelayCommand>(() => new RelayCommand(UpdateData));
            this._previousCommand = new Lazy<RelayCommand>(() => new RelayCommand(() => this.DisplayIndex--));
            this._nextCommand = new Lazy<RelayCommand>(() => new RelayCommand(() => this.DisplayIndex++));
        }

        private void UpdateData()
        {
            this.urls = this.db.QueryInvalidateGroup().Select(t => t.SourceUrl).ToList();

            this.Count = this.urls?.Count ?? 0;
            if (!string.IsNullOrEmpty(this.displayUrl) && urls?.Contains(this.displayUrl) == true)
            {
                this.DisplayIndex = this.urls.IndexOf(this.displayUrl);
            }
            else
            {
                this.DisplayIndex = 0;
            }
        }
    }
}
