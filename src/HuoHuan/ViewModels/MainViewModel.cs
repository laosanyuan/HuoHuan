using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HuoHuan.Enums;
using HuoHuan.Views.Pages;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace HuoHuan.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly Page home = new MainPage();
        private readonly Page view = new ViewPage();
        private readonly Page download = new DownloadPage();

        public MainViewModel()
        {
            this.Page = home;
        }

        #region [Properties]
        private Page page = null!;
        /// <summary>
        /// 子页面
        /// </summary>
        public Page Page
        {
            get => page;
            set => SetProperty(ref page, value);
        }
        #endregion

        #region [Commands]
        /// <summary>
        /// 切换页面
        /// </summary>
        public ICommand ChangePage => new Lazy<RelayCommand<PageType>>(() => new RelayCommand<PageType>(type =>
        {
            switch (type)
            {
                case PageType.Home:
                    this.Page = home;
                    break;
                case PageType.View:
                    this.Page = view;
                    break;
                case PageType.Download:
                    this.Page = download;
                    break;
            }
        })).Value;
        #endregion
    }
}
