using HuoHuan.Core.Install.UrlProviders;
using HuoHuan.Utils;
using Ninject;
using System.Configuration;

namespace HuoHuan.Core
{
    public static class LocalConfigManager
    {
        #region [Fields]
        private static readonly string _path = Path.Combine(FolderUtil.AppData, "configs.yml");
        #endregion

        private static readonly IKernel _ninjectKernel = new StandardKernel();
        public static IKernel NinjectKernel => _ninjectKernel;
        public static ConfigData Config { get; private set; } = null!;

        #region [Properties]
        /// <summary>
        /// 升级信息地址
        /// </summary>
        public static string UpdateInfoUrl
        {
            get
            {
                var key = Config?.InstallSource switch
                {
                    InstallType.Github => "GithubUpdateUrl",
                    InstallType.Gitee or _ => null,
                } ?? "GiteeUpdateUrl";

                return ConfigurationManager.AppSettings[key]!;
            }
        }
        /// <summary>
        /// 安装包下载地址
        /// </summary>

        public static string DownloadInstallUrl
        {
            get
            {
                var key = Config?.InstallSource switch
                {
                    InstallType.Github => "GithubDownloadUrl",
                    InstallType.Gitee or _ => null,
                } ?? "GiteeDownloadUrl";

                return ConfigurationManager.AppSettings[key]!;
            }
        }
        /// <summary>
        /// Gitee项目主页
        /// </summary>
        public static string GiteeUrl => ConfigurationManager.AppSettings["GiteeProjectUrl"]!;

        /// <summary>
        /// Github仓库主页
        /// </summary>
        public static string GithubUrl => ConfigurationManager.AppSettings["GithubProjectUrl"]!;
        #endregion

        static LocalConfigManager()
        {
            if (!File.Exists(_path))
            {
                Config ??= new();
                Save();
            }
            Config = YamlUtil.Deserializer<ConfigData>(File.ReadAllText(_path));

            var type = Config.InstallSource == InstallType.Gitee ? typeof(GiteeUrlProvider) : typeof(GithubUrlProvider);
            _ninjectKernel.Bind<IUrlProvider>().To(type).InSingletonScope();
        }

        #region [Methods]
        /// <summary>
        /// 保存配置
        /// </summary>
        public static void Save()
        {
            if (!File.Exists(_path))
            {
                File.Create(_path).Close();
            }
            File.WriteAllText(_path, YamlUtil.Serializer(Config));
        }
        #endregion
    }

    [Serializable]
    public class ConfigData
    {
        public InstallType InstallSource { get; set; }
    }

    public enum InstallType
    {
        Gitee,
        Github,
    }
}
