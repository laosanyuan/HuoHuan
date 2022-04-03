using HuoHuan.Models;
using System.Windows;
using System.Windows.Controls;

namespace HuoHuan.Control
{
    /// <summary>
    /// ImageCard.xaml 的交互逻辑
    /// </summary>
    public partial class ImageCard : UserControl
    {
        public ImageCard()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImageInfoProperty = DependencyProperty.Register(nameof(ImageInfo), typeof(DisplayImageInfo), typeof(ImageCard));
        /// <summary>
        /// 图片信息
        /// </summary>
        public DisplayImageInfo ImageInfo
        {
            get => (DisplayImageInfo)GetValue(ImageInfoProperty);
            set => SetValue(ImageInfoProperty, value);
        }
    }
}
