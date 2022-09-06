using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HuoHuan.Plugin
{
    internal static class PluginLoader
    {
        private static List<IPlugin> _plugins = null!;
        /// <summary>
        /// 已加载插件
        /// </summary>
        public static List<IPlugin> Plugins
        {
            get
            {
                _plugins ??= LoadPlugins();
                if (_plugins == null || _plugins.Count <= 0)
                {
                    return default!;
                }
                return PluginConfig.FilterPlugins(_plugins);
            }
        }

        /// <summary>
        /// 重置插件集合
        /// </summary>
        public static void Reset()
        {
            _plugins = null!;
            LoadPlugins();
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <returns></returns>
        private static List<IPlugin> LoadPlugins()
        {
            List<IPlugin> plugins = new();
            try
            {
                var types = Assembly.LoadFile(Path.Combine(FolderUtil.Current, "HuoHuan.Plugin.Plugins.dll")).GetTypes();
                var pluginType = typeof(IPlugin);

                foreach (var type in types)
                {
                    if (type.GetInterfaces().Any(t => t.FullName == pluginType.FullName))
                    {
                        IPlugin? plugin = Activator.CreateInstance(type) as IPlugin;
                        if (plugin is not null)
                        {
                            plugins.Add(plugin);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return plugins;
        }
    }
}
