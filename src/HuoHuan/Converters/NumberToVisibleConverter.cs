using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using YamlDotNet.Core.Tokens;

namespace HuoHuan.Converters
{
    /// <summary>
    /// 检查索引
    /// </summary>
    internal class NumberToVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var target = (int)values[0];

            var source = 0;
            if (values.Length > 1)
            {
                source = (int)values[1] - 1;
            }

            if (source != target)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
