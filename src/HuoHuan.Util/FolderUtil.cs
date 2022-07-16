namespace HuoHuan.Util
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
    }
}
