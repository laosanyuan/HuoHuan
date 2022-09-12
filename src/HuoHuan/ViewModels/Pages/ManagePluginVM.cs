using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HuoHuan.Plugin;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HuoHuan.ViewModels.Pages
{
    [ObservableObject]
    public partial class ManagePluginVM
    {
        #region [Dependency Properties]
        /// <summary>
        /// 差价信息集合
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<PluginConfigItem> _pluginItems = null!;
        /// <summary>
        /// 是否冻结当前操作
        /// </summary>
        [ObservableProperty]
        private bool _isFreezen;
        #endregion

        public ManagePluginVM()
        {
            StrongReferenceMessenger.Default.Register<string, string>(this, "RunningStatus", (r, isRunning) =>
            {
                this.IsFreezen = Convert.ToBoolean(isRunning);
            });
            this.PluginItems = new ObservableCollection<PluginConfigItem>(PluginConfig.Load());
            this.PluginItems.CollectionChanged += (sender, e) => this.UpdateGlobalPluginsConfig();
        }

        #region [Commands]
        /// <summary>
        /// 插件移除加入取反
        /// </summary>
        /// <param name="item"></param>
        [RelayCommand]
        private void Operation(object item)
        {
            if (item is PluginConfigItem plugin)
            {
                this.PluginItems.First(x => x.Name == plugin.Name).IsEnabled = !plugin.IsEnabled;
                this.UpdateGlobalPluginsConfig();
            }
        }

        #endregion

        /// <summary>
        /// 更新全局插件配置变化
        /// </summary>
        private void UpdateGlobalPluginsConfig()
        {
            PluginConfig.Save(this.PluginItems);
            PluginLoader.Reset();
            StrongReferenceMessenger.Default.Send<object, string>("UpdatePluginList");
        }
    }
}
