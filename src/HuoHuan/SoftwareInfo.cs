using System.Reflection;

namespace HuoHuan
{
    public class SoftwareInfo
    {
        public static string Identity => "HuoHuan";
        public static string Name => "火浣";
        public static string Description => "群聊耙子";
        public static string Version
        {
            get
            {
                var fullVersion = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString();
                if (fullVersion == null)
                {
                    return "1.0.0";
                }
                else
                {
                    fullVersion.LastIndexOf('.');
                    return fullVersion[..fullVersion.LastIndexOf('.')];
                }
            }
        }
        public static string LogoPath => "/Resources/HuoHuan.png";
    }
}
