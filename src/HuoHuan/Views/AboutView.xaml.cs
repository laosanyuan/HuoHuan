using HuoHuan.Core;
using HuoHuan.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HuoHuan.Views
{
    /// <summary>
    /// AboutView.xaml 的交互逻辑
    /// </summary>
    public partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();

            this.MouseMove += (_, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };

            this.github.Text = LocalConfigManager.GithubUrl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock text)
            {
                WebUtil.OpenUrl(text.Text);
            }
        }
    }
}
