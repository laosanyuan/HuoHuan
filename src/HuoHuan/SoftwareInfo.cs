using System.Reflection;

namespace HuoHuan
{
    public static class SoftwareInfo
    {
        public static readonly string Identity = "HuoHuan";
        public static readonly string Name = "火浣";
        public static readonly string Description = "群聊耙子";
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
        public static readonly string LogoPath = "/Resources/HuoHuan.png";
    }
}
