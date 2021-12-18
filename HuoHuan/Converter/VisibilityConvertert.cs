using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HuoHuan.Converter
{
    /// <summary>
    /// 根据对象是否为空判断显隐
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 是否为空时显示
            bool isNullVisible = Boolean.TrueString.ToLower().Equals(parameter.ToString().ToLower());

            return isNullVisible
                ? value == null ? Visibility.Visible : Visibility.Hidden
                : (object)(value == null ? Visibility.Hidden : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
