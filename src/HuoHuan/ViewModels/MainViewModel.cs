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
        #region [Fields]
        private readonly Page home = new MainPage();
        private readonly Page view = new ViewPage();
        private readonly Page download = new DownloadPage();
        #endregion

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
        private readonly Lazy<RelayCommand<PageType>> _changePageCommand;
        /// <summary>
        /// 切换页面
        /// </summary>
        public ICommand ChangePage => _changePageCommand.Value;
        #endregion

        public MainViewModel()
        {
            this.Page = home;
            this._changePageCommand = new Lazy<RelayCommand<PageType>>(() => new RelayCommand<PageType>(type =>
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
            }));
        }
    }
}
