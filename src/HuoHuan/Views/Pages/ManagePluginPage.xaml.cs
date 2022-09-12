using System.Windows.Controls;

namespace HuoHuan.Views.Pages
{
    /// <summary>
    /// ManagePluginPage.xaml 的交互逻辑
    /// </summary>
    public partial class ManagePluginPage : Page
    {
        public ManagePluginPage()
        {
            InitializeComponent();
            this.MouseMove += (_, e) => e.Handled = true;
        }
    }
}
