using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace HuoHuan.Controls
{
    /// <summary>
    /// 图片展示控件
    /// </summary>
    internal class ImageCard : Control
    {
        public static readonly DependencyProperty IsValidProperty 
            = DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(ImageCard));
        /// <summary>
        /// 是否为有效图
        /// </summary>
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        public static readonly DependencyProperty IsDownloadedProperty 
            = DependencyProperty.Register(nameof(IsDownloaded), typeof(bool), typeof(ImageCard));
        /// <summary>
        /// 是否已下载
        /// </summary>
        public bool IsDownloaded
        {
            get => (bool)GetValue(IsDownloadedProperty);
            set => SetValue(IsDownloadedProperty, value);
        }

        public static DependencyProperty BitmapProperty
            = DependencyProperty.Register(nameof(Bitmap), typeof(Bitmap), typeof(ImageCard));
        /// <summary>
        /// 展示图片
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public Bitmap Bitmap
        {
            get => (Bitmap)GetValue(BitmapProperty);
            set => SetValue(BitmapProperty, value);
        }
    }

    /// <summary>
    /// Bitmap转换为Image能使用的BitmapImage
    /// </summary>
    internal class BitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Bitmap bitmap)
            {
                return default!;
            }
            BitmapImage image = new();
            using MemoryStream ms = new();
            bitmap.Save(ms, ImageFormat.Bmp);
            var bytes = ms.GetBuffer();
            image.BeginInit();
            image.StreamSource = new MemoryStream(bytes);
            image.EndInit();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
