using HuoHuan.Core.Install.UrlProviders;
using NUnit.Framework;
using System.Configuration;

namespace HuoHuan.Core.Test
{
    public class UrlProviderTest
    {
        [Test]
        public void TestGiteeGetUrl()
        {
            ConfigurationManager.AppSettings["GiteeDownloadUrl"] = "https://gitee.com/ylaosan/huo-huan/releases";
            var provider = new GiteeUrlProvider();
            string url = provider.GetDownloadUrl("1.0.0").Result;
            Assert.That(url, Is.EqualTo("https://gitee.com/ylaosan/huo-huan/releases/download/1.0.0/Setup.exe"));
        }

        [Test]
        public void TestGithubGetUrl()
        {
            ConfigurationManager.AppSettings["GithubDownloadUrl"] = "https://github.com/laosanyuan/HuoHuan/releases";
            var provider = new GithubUrlProvider();
            string url = provider.GetDownloadUrl("1.0.0").Result;
            Assert.That(url, Is.EqualTo("https://github.com/laosanyuan/HuoHuan/releases/download/1.0.0/Setup.exe"));
        }
    }
}
