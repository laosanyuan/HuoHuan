using HuoHuan.ViewModel.Pages;
using System.Windows;
using System.Windows.Controls;

namespace HuoHuan.View.Pages
{
    /// <summary>
    /// ViewPage.xaml 的交互逻辑
    /// </summary>
    public partial class ViewPage : Page
    {
        public ViewPage()
        {
            InitializeComponent();
            this.IsVisibleChanged += ViewPage_IsVisibleChanged;
        }

        private void ViewPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                (this.DataContext as ViewPageVM)?.RefreshDataCommand?.Execute(default);
            }
        }
    }
}
