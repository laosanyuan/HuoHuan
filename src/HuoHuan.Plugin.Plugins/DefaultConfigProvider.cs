using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.Text;

namespace HuoHuan.Plugin.Plugins
{
    internal static class DefaultConfigProvider
    {
        private static readonly EmbeddedFileProvider _embeddedFileProvider = new EmbeddedFileProvider(Assembly.GetCallingAssembly());

        internal static Stream GetFileStyream(string name)
        {
            var res = _embeddedFileProvider.GetFileInfo(name);
            if (res.Exists)
            {
                return res.CreateReadStream();
            }
            return default!;
        }

        /// <summary>
        /// 根据文件路径获取嵌入文件文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static string GetFileByPath(string filePath)
        {
            using Stream stream = GetFileStyream(filePath);
            using StreamReader reader = new StreamReader(stream);
            var bytes = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            return Encoding.Unicode.GetString(Encoding.Convert(Encoding.UTF8, Encoding.Unicode, bytes));
        }
    }
}
