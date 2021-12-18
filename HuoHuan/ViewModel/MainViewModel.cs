using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HuoHuan.Core.Spider;
using HuoHuan.Data;
using HuoHuan.Enum;
using HuoHuan.Models;
using HuoHuan.View.Pages;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace HuoHuan.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Page home = new MainPage();
        private readonly Page view = new ViewPage();
        private readonly Page download = new DownloadPage();
     
        public MainViewModel()
        {
            this.Page = home;
        }

        #region [Properties]
        private Page page;
        /// <summary>
        /// ×ÓÒ³Ãæ
        /// </summary>
        public Page Page
        {
            get => page;
            set
            {
                page = value;
                RaisePropertyChanged(nameof(Page), value);
            }
        }
        #endregion

        #region [Commands]
        /// <summary>
        /// ÇÐ»»Ò³Ãæ
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