using HuoHuan.Controls;
using HuoHuan.Core.Enums;
using HuoHuan.Enums;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace HuoHuan.Views.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            this.MouseMove += (_, e) => e.Handled = true;
        }

        private void start_button_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (((SwitchButton)sender).IsChecked == true)
            {
                this.pause_button.IsChecked = false;
            }
        }
    }

    /// <summary>
    /// 启停转换
    /// </summary>
    public class StartStopConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? SpiderOperationStatus.Start : SpiderOperationStatus.Stop;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 暂停继续转换
    /// </summary>
    public class PauseContinueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? SpiderOperationStatus.Pause : SpiderOperationStatus.Continue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
