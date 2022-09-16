using HuoHuan.Core.Install.UrlProviders;
using NUnit.Framework;

namespace HuoHuan.Core.Test
{
    public class UrlProviderTest
    {
        [Test]
        public void TestGiteeGetUrl()
        {
            var provider = new GiteeUrlProvider();
            string url = provider.GetDownloadUrl("https://gitee.com/ylaosan/huo-huan/releases", "1.0.0").Result;
            Assert.That(url, Is.EqualTo("https://gitee.com/ylaosan/huo-huan/releases/download/1.0.0/Setup.exe"));
        }

        [Test]
        public void TestGithubGetUrl()
        {
            var provider = new GithubUrlProvider();
            string url = provider.GetDownloadUrl("https://github.com/laosanyuan/HuoHuan/releases", "1.0.0").Result;
            Assert.That(url, Is.EqualTo("https://github.com/laosanyuan/HuoHuan/releases/download/1.0.0/Setup.exe"));
        }
    }
}
