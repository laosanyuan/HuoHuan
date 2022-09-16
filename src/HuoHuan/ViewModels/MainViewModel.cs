using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.Enums;
using HuoHuan.Views;
using HuoHuan.Views.Pages;
using System.Windows.Controls;

namespace HuoHuan.ViewModels
{
    [ObservableObject]
    public partial class MainViewModel
    {
        #region [Fields]
        private readonly Page _home = new HomePage();
        private readonly Page _view = new ViewPage();
        private readonly Page _manage = new ManagePluginPage();
        #endregion

        #region [Properties]
        /// <summary>
        /// 子页面
        /// </summary>
        [ObservableProperty]
        private Page _page = null!;
        #endregion

        public MainViewModel()
        {
            this.Page = _home;
        }

        #region [Commands]
        /// <summary>
        /// 切换页面
        /// </summary>
        /// <param name="type"></param>
        [RelayCommand]
        private void ChangePage(PageType type)
        {
            this.Page = type switch
            {
                PageType.View => _view,
                PageType.ManagePlugin => _manage,
                PageType.Home or _ => _home,
            };
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="settings"></param>
        [RelayCommand]
        private void SettingPage(SettingType settings)
        {
            switch (settings)
            {
                case SettingType.About:
                    new AboutView().ShowDialog();
                    break;
                case SettingType.Setting:
                _:
                    break;
            }
        }
        #endregion
    }
}
