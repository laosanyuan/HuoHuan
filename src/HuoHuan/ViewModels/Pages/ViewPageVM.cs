using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.DataBase.Models;
using HuoHuan.DataBase.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HuoHuan.ViewModels.Pages
{
    [ObservableObject]
    public partial class ViewPageVM
    {
        #region [Fields]
        private readonly GroupDB _db = new();           // 微信群链接数据库
        private List<string> _urls = null!;             // 
        private List<GroupImage> _cacheImages = null!;  // 数据库查询结果缓存
        #endregion

        #region [Dependency Properties]
        /// <summary>
        /// 显示Url
        /// </summary>
        [ObservableProperty]
        private string _displayUrl = SoftwareInfo.LogoPath;

        private int _displayIndex;
        /// <summary>
        /// 显示图片索引
        /// </summary>
        public int DisplayIndex
        {
            get => _displayIndex;
            set
            {
                if (value == this._displayIndex && !_displayUrl.Equals(SoftwareInfo.LogoPath))
                {
                    return;
                }
                if (value < 0 || this._count == 0)
                {
                    value = 0;
                }
                else if (value > _urls.Count - 1)
                {
                    value = _urls.Count - 1;
                }

                SetProperty(ref _displayIndex, value);
                this.RefreshViewUrl();
            }
        }
        /// <summary>
        /// 图片数量
        /// </summary>
        [ObservableProperty]
        private int _count;

        /// <summary>
        /// 开始切换动画
        /// </summary>
        [ObservableProperty]
        private bool _startAnimation;
        #endregion

        #region [Commands]
        /// <summary>
        /// 刷新数据
        /// </summary>
        [RelayCommand]
        private async Task RefreshData()
        {
            this._cacheImages = await this._db.QueryValidateGroup();
            this._urls = this._cacheImages.Select(t => t.FullName).ToList();

            this.Count = this._urls?.Count ?? 0;
            if (!string.IsNullOrEmpty(this._displayUrl) && this._urls?.Contains(this._displayUrl) == true)
            {
                this.DisplayIndex = this._urls.IndexOf(this._displayUrl);
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

        /// <summary>
        /// 删除
        /// </summary>
        [RelayCommand]
        private async void Delete()
        {
            if (this.Count <= 0)
            {
                return;
            }

            var tmpUrl = this.DisplayUrl;

            if (this.DisplayIndex >= Count - 1)
            {
                this.DisplayIndex--;
            }
            await this._db.DeleteUrl(this._cacheImages?.First(t => t.FullName.Equals(tmpUrl))?.Url!);
            this._urls?.Remove(tmpUrl);
            this.Count--;
            this.RefreshViewUrl();
        }

        /// <summary>
        /// 重置切换动画标记
        /// </summary>
        [RelayCommand]
        private void ResetAnimationFlag() => this.StartAnimation = false;
        #endregion

        // 刷新正在显示图片
        private void RefreshViewUrl()
        {
            this.DisplayUrl =
                this._count == 0 ? SoftwareInfo.LogoPath : this._urls[this._displayIndex];

            this.StartAnimation = true;
        }
    }
}
