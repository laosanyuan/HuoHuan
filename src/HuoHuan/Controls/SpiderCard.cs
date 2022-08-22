using AngleSharp.Browser;
using HuoHuan.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HuoHuan.Controls
{
    /// <summary>
    /// 爬取器展示控件
    /// </summary>
    internal class SpiderCard : Control
    {
        public static readonly DependencyProperty SpiderInfoProperty
            = DependencyProperty.Register(nameof(SpiderInfo), typeof(SpiderInfo), typeof(SpiderCard));
        /// <summary>
        /// 爬取器
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public SpiderInfo SpiderInfo
        {
            get => (SpiderInfo)GetValue(SpiderInfoProperty);
            set => SetValue(SpiderInfoProperty, value);
        }

        public static readonly DependencyProperty StartCommandProperty
            = DependencyProperty.Register("StartCommand", typeof(ICommand), typeof(SpiderCard));
        /// <summary>
        /// 开始
        /// </summary>
        public ICommand StartCommand
        {
            get => (ICommand)GetValue(StartCommandProperty);
            set => SetValue(StartCommandProperty, value);
        }

        public static readonly DependencyProperty StopCommandProperty
            = DependencyProperty.Register("StopCommand", typeof(ICommand), typeof(SpiderCard));
        /// <summary>
        /// 停止
        /// </summary>
        public ICommand StopCommand
        {
            get => (ICommand)GetValue(StopCommandProperty);
            set => SetValue(StopCommandProperty, value);
        }

        public static readonly DependencyProperty PauseCommandProperty
            = DependencyProperty.Register("PauseCommand", typeof(ICommand), typeof(SpiderCard));
        /// <summary>
        /// 暂停
        /// </summary>
        public ICommand PauseCommand
        {
            get => (ICommand)GetValue(PauseCommandProperty);
            set => SetValue(PauseCommandProperty, value);
        }

        public static readonly DependencyProperty ContinueCommandProperty
            = DependencyProperty.Register("ContinueCommand", typeof(ICommand), typeof(SpiderCard));
        /// <summary>
        /// 继续
        /// </summary>
        public ICommand ContinueCommand
        {
            get => (ICommand)GetValue(ContinueCommandProperty);
            set => SetValue(ContinueCommandProperty, value);
        }
    }
}
