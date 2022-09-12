using System.Windows;

namespace HuoHuan.Views
{
    /// <summary>
    /// UpgradeView.xaml 的交互逻辑
    /// </summary>
    public partial class UpgradeView : Window
    {
        public UpgradeView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
