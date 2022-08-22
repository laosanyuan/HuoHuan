using HuoHuan.Utils;
using System.Text;

namespace HuoHuan.Plugin.Contracs
{
    public interface IConfig
    {
        /// <summary>
        /// 加载
        /// </summary>
        public void Load();
        /// <summary>
        /// 保存
        /// </summary>
        public void Save();
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset();
    }

    public abstract class BaseConfig<T> : IConfig
    {
        /// <summary>
        /// 配置内容
        /// </summary>
        public abstract T Config { get; protected set; }
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 配置文件保存路径
        /// </summary>
        protected string FilePath => Path.Combine(FolderUtil.ConfigPath, this.Name);

        /// <summary>
        /// 加载配置
        /// </summary>
        public virtual async void Load()
        {
            var file = Path.Combine(FolderUtil.ConfigPath, this.Name);
            if (!File.Exists(file))
            {
                File.Create(file);
            }
            else
            {
                var str = await File.ReadAllTextAsync(file, Encoding.UTF8);
                this.Config = YamlUtil.Deserializer<T>(str);
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public virtual void Save()
        {
            var str = YamlUtil.Serializer(this.Config);
            File.WriteAllTextAsync(Path.Combine(FolderUtil.AppData, this.Name), str, Encoding.UTF8);
        }

        /// <summary>
        /// 恢复默认配置
        /// </summary>
        public abstract void Reset();
    }
}
