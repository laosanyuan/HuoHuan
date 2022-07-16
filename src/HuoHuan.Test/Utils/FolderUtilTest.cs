using HuoHuan.Util;
using NUnit.Framework;
using System.IO;

namespace HuoHuan.Test.Utils
{
    [TestFixture]
    internal class TestFolderUtil
    {
        [Test]
        public void AppDataTest()
        {
            var path = FolderUtil.AppData;
            Assert.IsTrue(Directory.Exists(path));
        }
    }
}
