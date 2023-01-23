using HuoHuan.Core.Install.UrlProviders;
using Ninject;
using System.Configuration;

namespace HuoHuan.Core
{
    public static class LocalConfigManager
    {
        private static readonly IKernel _ninjectKernel = new StandardKernel();
        public static IKernel NinjectKernel => _ninjectKernel;

        #region [Properties]
        /// <summary>
        /// 升级信息地址
        /// </summary>
        public static string UpdateInfoUrl => ConfigurationManager.AppSettings["GithubUpdateUrl"]!;

        /// <summary>
        /// 安装包下载地址
        /// </summary>

        public static string DownloadInstallUrl => ConfigurationManager.AppSettings["GithubDownloadUrl"]!;

        /// <summary>
        /// Github仓库主页
        /// </summary>
        public static string GithubUrl => ConfigurationManager.AppSettings["GithubProjectUrl"]!;
        #endregion

        static LocalConfigManager()
        {
            _ninjectKernel.Bind<IUrlProvider>().To(typeof( GithubUrlProvider)).InSingletonScope();
        }
    }
}
