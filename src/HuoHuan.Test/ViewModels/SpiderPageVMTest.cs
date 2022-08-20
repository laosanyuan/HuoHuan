using HuoHuan.ViewModels.Pages;
using NUnit.Framework;

namespace HuoHuan.Test.ViewModels
{
    [TestFixture]
    public class SpiderPageVMTest
    {
        [Test]
        public void TestLoadSpider()
        {
            var vm = new HomePageVM();
            vm.StartCommand.Execute(null);

            Assert.IsTrue(vm.SpiderInfos is not null);
        }
    }
}
