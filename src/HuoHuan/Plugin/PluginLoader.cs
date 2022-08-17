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
        public static List<IPlugin> Plugins
        {
            get
            {
                if (_plugins is null)
                {
                    _plugins = LoadPlugins();
                }
                return _plugins;
            }
        }

        public static void Reset()
        {
            _plugins = null!;
            LoadPlugins();
        }

        private static List<IPlugin> LoadPlugins()
        {
            List<IPlugin> plugins = new List<IPlugin>();
            try
            {
                var types = Assembly.LoadFile(Path.Combine(FolderUtil.Current, "HuoHuan.Plugins.dll")).GetTypes();
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
