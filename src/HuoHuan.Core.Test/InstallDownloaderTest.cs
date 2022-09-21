using HuoHuan.Core.Install;
using NUnit.Framework;

namespace HuoHuan.Core.Test
{
    public class InstallDownloaderTest
    {
        // CI²»±Ø²âÏÂÔØ

        [Test]
        public void TestDownload()
        {
            //bool isFinish = false;
            //var downloader = new InstallDownloader();
            //downloader.DownloadProgressChanged += (sender, e) =>
            //{
            //    if (e.IsEnded && e.AllSize == e.DownloadedSize)
            //    {
            //        isFinish = true;
            //    }
            //};
            //downloader.DownloadAsync(
            //    "https://gitee.com/ylaosan/huo-huan/releases/download/1.0.0/Setup.exe",
            //    "test.exe",
            //    new CancellationToken()).Wait();

            //Assert.IsTrue(isFinish);
        }

        [Test]
        public void TestCancelDownload()
        {
            //bool isBreak = false;
            //var downloader = new InstallDownloader();

            //CancellationTokenSource source = new();
            //CancellationToken token = source.Token;

            //downloader.DownloadProgressChanged += (sender, e) =>
            //{
            //    source.Cancel();
            //    if (e.IsEnded && e.AllSize != e.DownloadedSize)
            //    {
            //        isBreak = true;
            //    }
            //};
            //downloader.DownloadAsync(
            //    "https://gitee.com/ylaosan/huo-huan/releases/download/1.0.0/Setup.exe",
            //    "test.exe",
            //    token).Wait();

            //Assert.IsTrue(isBreak);
        }
    }
}