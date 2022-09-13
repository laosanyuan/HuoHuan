using System.Windows;
using System.Windows.Input;

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

            this.MouseMove += (_, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
