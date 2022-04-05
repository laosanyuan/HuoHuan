using HuoHuan.Controls;
using System.Windows.Controls;

namespace HuoHuan.Views.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void start_button_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (((SwitchButton)sender).IsChecked == true)
            {
                this.pause_button.IsChecked = false;
            }
        }

        private void First_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.start_area.Visibility = System.Windows.Visibility.Hidden;
            this.run_area.Visibility = System.Windows.Visibility.Visible;

            this.start_button.IsChecked = true;
            this.start_button.Command?.Execute(true);
        }
    }
}
