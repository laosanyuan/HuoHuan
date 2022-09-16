namespace HuoHuan.Core.Install.UrlProviders
{
    public interface IUrlPriovider
    {
        Task<string> GetDownloadUrl(string url,string version);
    }
}
