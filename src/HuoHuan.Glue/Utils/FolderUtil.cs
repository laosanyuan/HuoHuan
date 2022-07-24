namespace HuoHuan.Glue.Utils
{
    public class FolderUtil
    {
        public static string AppData
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = Path.Combine(appDataPath, SoftwareInfo.Identity);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        /// <summary>
        /// 数据库路径
        /// </summary>
        public static string DbPath
        {
            get
            {
                var path = Path.Combine(AppData, "db");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        /// <summary>
        /// 运行路径
        /// </summary>
        public static string Current => Environment.CurrentDirectory;
    }
}
