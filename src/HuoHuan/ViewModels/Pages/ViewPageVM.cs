using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.DataBase.Services;
using HuoHuan.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HuoHuan.ViewModels.Pages
{
    [ObservableObject]
    public partial class ViewPageVM
    {
        #region [Fields]
        private readonly GroupDB db = new();    // 微信群链接数据库
        private List<string> urls = null!;
        #endregion

        #region [Dependency Properties]
        /// <summary>
        /// 显示Url
        /// </summary>
        [ObservableProperty]
        private string _displayUrl = SoftwareInfo.LogoPath;

        private int displayIndex;
        /// <summary>
        /// 显示图片索引
        /// </summary>
        public int DisplayIndex
        {
            get => displayIndex;
            set
            {
                if (value == this.displayIndex && !_displayUrl.Equals(SoftwareInfo.LogoPath))
                {
                    return;
                }
                if (value < 0 || this._count == 0)
                {
                    value = 0;
                }
                else if (value > urls.Count - 1)
                {
                    value = urls.Count - 1;
                }
                this.DisplayUrl = this._count == 0 ? SoftwareInfo.LogoPath : this.urls[value];

                SetProperty(ref displayIndex, value);
            }
        }
        /// <summary>
        /// 图片数量
        /// </summary>
        [ObservableProperty]
        private int _count;
        #endregion

        #region [Commands]
        /// <summary>
        /// 刷新数据
        /// </summary>
        [RelayCommand]
        private async Task RefreshData()
        {
            this.urls = (await this.db.QueryValidateGroup()).Select(t => t.FullName).ToList();

            this.Count = this.urls?.Count ?? 0;
            if (!string.IsNullOrEmpty(this._displayUrl) && urls?.Contains(this._displayUrl) == true)
            {
                this.DisplayIndex = this.urls.IndexOf(this._displayUrl);
            }
            else
            {
                this.DisplayIndex = 0;
            }
        }

        /// <summary>
        /// 前一张
        /// </summary>
        [RelayCommand]
        private void Previous() => this.DisplayIndex--;

        /// <summary>
        /// 后一张
        /// </summary>
        [RelayCommand]
        private void Next() => this.DisplayIndex++;
        #endregion
    }
}
