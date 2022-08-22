using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.Enums;
using HuoHuan.Views.Pages;
using System.Windows.Controls;
using System.Windows.Input;

namespace HuoHuan.ViewModels
{
    [ObservableObject]
    public partial class MainViewModel
    {
        #region [Fields]
        private readonly Page _home = new HomePage();
        private readonly Page _view = new ViewPage();
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
        [ICommand]
        private void ChangePage(PageType type)
        {
            this.Page = type switch
            {
                PageType.View => _view,
                PageType.Home or _ => _home,
            };
        }
        #endregion
    }
}
