using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HuoHuan.Controls
{
    internal class ImageCard : Control
    {
        public static readonly DependencyProperty IsValidProperty 
            = DependencyProperty.Register("IsValid", typeof(bool), typeof(ImageCard));
        /// <summary>
        /// 是否为有效图
        /// </summary>
        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty IsDownloadedProperty 
            = DependencyProperty.Register("IsDownloaded", typeof(bool), typeof(ImageCard));
        /// <summary>
        /// 是否已下载
        /// </summary>
        public bool IsDownloaded
        {
            get { return (bool)GetValue(IsDownloadedProperty); }
            set { SetValue(IsDownloadedProperty, value); }
        }

        public static readonly DependencyProperty UrlProperty 
            = DependencyProperty.Register("Url", typeof(ImageSource), typeof(ImageCard));
        /// <summary>
        /// 图片链接
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public ImageSource Url
        {
            get { return (ImageSource)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }
    }
}
