using CommunityToolkit.Mvvm.Messaging;
using HuoHuan.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace HuoHuan.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MouseMove += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 检查版本更新
            var url = ConfigurationManager.AppSettings["UpdateUrl"];
            var client = new HttpClient()
            {
                Timeout = TimeSpan.FromMinutes(1),
            };
            try
            {
                var response = await client.GetAsync(url);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var str = await response.Content.ReadAsStringAsync();
                    var newVersion = YamlUtil.Deserializer<VersionInfo>(str);
                    if (!newVersion.Version.Trim().Equals(SoftwareInfo.Version))
                    {
                        WeakReferenceMessenger.Default.Send(newVersion, "UpgradeVersion");
                        await App.Current.Dispatcher.InvokeAsync(() =>
                        {
                            new UpgradeView().ShowDialog();
                        });
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            this.pop.IsOpen = true;
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        public class VersionInfo
        {
            public string Version { get; init; } = null!;
            public string Title { get; init; } = null!;
            public List<string> Messages { get; init; } = null!;
        }
    }
}
