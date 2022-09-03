namespace HuoHuan.Plugin.Plugins.Test
{
    internal class Jiaqun91SpiderTest
    {
        [Test]
        public void TestStart()
        {
            Jiaqun91Spider spider = new Jiaqun91Spider();
            bool crawledFlag = false;
            bool progressFlag = false;
            spider.ProgressStatusChanged += (sender, e) =>
            {
                if (e.Progress > 0)
                {
                    progressFlag = true;
                }
            };
            spider.Crawled += (sender, e) =>
            {
                crawledFlag = true;
            };
            Assert.That(spider.Status == Contracs.SpiderStatus.Waiting, Is.True);
            spider.Start();
            Assert.That(spider.Status == Contracs.SpiderStatus.Running, Is.True);
            bool checkResult = false;
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(10);
                    if (progressFlag && crawledFlag)
                    {
                        checkResult = true;
                        return Task.CompletedTask;
                    }
                }
            }).Wait(60 * 1000);
            Assert.That(checkResult, Is.True);
        }
    }
}
