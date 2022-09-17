using Ninject;

namespace HuoHuan.Core.Install.UrlProviders
{
    public interface IUrlProvider
    {
        Task<string> GetDownloadUrl(string version);
    }

    public static class UrlProvider
    {
        public static IUrlProvider Instance => LocalConfigManager.NinjectKernel.Get<IUrlProvider>();
    }
}
