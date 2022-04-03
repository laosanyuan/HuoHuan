using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HuoHuan.Control
{
    public class SwitchButton : CheckBox
    {
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size), typeof(double), typeof(SwitchButton));
        /// <summary>
        /// 按钮大小
        /// </summary>
        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty OpenIconProperty = DependencyProperty.Register(nameof(OpenIcon), typeof(string), typeof(SwitchButton));
        /// <summary>
        /// 开启状态图标
        /// </summary>
        public string OpenIcon
        {
            get => (string)GetValue(OpenIconProperty);
            set => SetValue(OpenIconProperty, value);
        }

        public static readonly DependencyProperty CloseIconProperty = DependencyProperty.Register(nameof(CloseIcon), typeof(string), typeof(SwitchButton));
        /// <summary>
        /// 关闭状态图标
        /// </summary>
        public string CloseIcon
        {
            get => (string)GetValue(CloseIconProperty);
            set => SetValue(CloseIconProperty, value);
        }

        public static readonly DependencyProperty OpenForegroundProperty = DependencyProperty.Register(nameof(OpenForeground), typeof(Brush), typeof(SwitchButton));
        /// <summary>
        /// 开启状态前景色
        /// </summary>
        public Brush OpenForeground
        {
            get => (Brush)GetValue(OpenForegroundProperty);
            set => SetValue(OpenForegroundProperty, value);
        }

        public static readonly DependencyProperty CloseForegroundProperty = DependencyProperty.Register(nameof(CloseForeground), typeof(Brush), typeof(SwitchButton));
        /// <summary>
        /// 关闭状态前景色
        /// </summary>
        public Brush CloseForeground
        {
            get => (Brush)GetValue(CloseForegroundProperty);
            set => SetValue(CloseForegroundProperty, value);
        }
    }
}
