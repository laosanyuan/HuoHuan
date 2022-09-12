using HuoHuan.ViewModels.Pages;
using System.Windows.Controls;

namespace HuoHuan.Views.Pages
{
    /// <summary>
    /// ViewPage.xaml 的交互逻辑
    /// </summary>
    public partial class ViewPage : Page
    {
        public ViewPage()
        {
            InitializeComponent();
            this.MouseMove += (_, e) => e.Handled = true;
            this.IsVisibleChanged += (_, e) =>
            {
                if ((bool)e.NewValue)
                {
                    (this.DataContext as ViewPageVM)?.RefreshDataCommand?.Execute(default);
                }
            };
        }
    }
}
