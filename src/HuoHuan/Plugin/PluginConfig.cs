using HuoHuan.Plugin.Contracs;
using HuoHuan.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HuoHuan.Plugin
{
    internal static class PluginConfig
    {
        private static readonly string _path = Path.Combine(FolderUtil.ConfigPath, "plugins.yml");

        /// <summary>
        /// 加载本地配置
        /// </summary>
        /// <returns></returns>
        public static List<PluginConfigItem> Load()
        {
            if (!File.Exists(_path))
            {
                return default!;
            }
            return YamlUtil.Deserializer<List<PluginConfigItem>>(File.ReadAllText(_path));
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="items"></param>
        public static void Save(List<PluginConfigItem> items)
        {
            if (!File.Exists(_path))
            {
                File.Create(_path).Close();
            }
            File.WriteAllText(_path, YamlUtil.Serializer(items));
        }

        /// <summary>
        /// 过滤插件
        /// </summary>
        /// <param name="plugins"></param>
        /// <returns></returns>
        public static List<IPlugin> FilterPlugins(IList<IPlugin> plugins)
        {
            var configs = Load();
            if (configs is null)
            {
                configs = new();
                foreach (var plugin in plugins)
                {
                    configs.Add(new PluginConfigItem()
                    {
                        Name = plugin.Name,
                        Description = plugin.Description,
                        IsEnabled = true,
                    });
                }
                Save(configs);
            }

            // 追加新增插件
            foreach (var plugin in plugins.Where(t => !configs.Any(z => z.Name.Equals(t.Name))))
            {
                configs.Add(new PluginConfigItem()
                {
                    Name = plugin.Name,
                    Description = plugin.Description,
                    IsEnabled = true,
                });
            }
            List<IPlugin> results = new();              // 返回结果
            List<PluginConfigItem> deletions = new();   // 待删除
            // 排序
            foreach (var config in configs)
            {
                if (plugins.Any(t => t.Name.Equals(config.Name)))
                {
                    if (config.IsEnabled)
                    {
                        results.Add(plugins.First(t => t.Name.Equals(config.Name)));
                    }
                }
                else
                {
                    deletions.Add(config);
                }
            }
            // 删除失效插件
            deletions.ForEach(t => configs.Remove(t));
            Save(configs);

            return results;
        }
    }

    [Serializable]
    internal class PluginConfigItem
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = null!;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
