using HuoHuan.Utils;
using Newtonsoft.Json;
using System.Text;

namespace HuoHuan.Plugin.Contracs
{
    public interface IConfig
    {
        public void Load();
        public void Save();
    }

    public abstract class BaseConfig<T> : IConfig
    {
        public abstract T Config { get; protected set; }
        public abstract string Name { get; }

        public async void Load()
        {
            var file = Path.Combine(FolderUtil.AppData, this.Name);
            if (!File.Exists(file))
            {
                File.Create(file);
            }
            else
            {
                var str = await File.ReadAllTextAsync(file, Encoding.UTF8);
                this.Config = JsonConvert.DeserializeObject<T>(str);
            }
        }

        public void Save()
        {
            var str = JsonConvert.SerializeObject(this.Config);
            File.WriteAllTextAsync(Path.Combine(FolderUtil.AppData, this.Name), str, Encoding.UTF8);
        }
    }
}
