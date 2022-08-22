
namespace HuoHuan.Utils
{
    public class FolderUtil
    {
        public static string AppData
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = Path.Combine(appDataPath, SoftwareInfo.Identity);
                return CreatePath(path);
            }
        }

        /// <summary>
        /// 数据库路径
        /// </summary>
        public static string DbPath => CreatePath(Path.Combine(AppData, "db"));

        /// <summary>
        /// 配置文件保存路径
        /// </summary>
        public static string ConfigPath => CreatePath(Path.Combine(AppData, "configs"));

        /// <summary>
        /// 图片保存路径
        /// </summary>
        public static string ImagesFolder
            => CreatePath(Path.Combine(AppData, "images", DateTime.Today.ToString("yyyy-MM-dd")));

        /// <summary>
        /// 运行路径
        /// </summary>
        public static string Current => Environment.CurrentDirectory;

        private static string CreatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
