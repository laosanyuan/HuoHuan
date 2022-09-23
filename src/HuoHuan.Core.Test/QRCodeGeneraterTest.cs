using HuoHuan.DataBase.Models;
using NUnit.Framework;

namespace HuoHuan.Core.Test
{
    public class QRCodeGeneraterTest
    {
        private QRCodeGenerater _generater = null!;
        private const string Folder = "TestQR";

        [SetUp]
        public void Setup()
        {
            if (Directory.Exists(Folder))
            {
                Directory.Delete(Folder, true);
            }
            _generater = new QRCodeGenerater(Folder, "HuoHuan.png");
        }

        [Test]
        public void TestGenerateImage()
        {
            TestName(new GroupImage() { QRText = "112233" }, null!, "112233.jpg");
            TestName(new GroupImage() { QRText = "1122zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz33" },
                "test_qr1.jpg",
                "test_qr1.jpg");
            TestName(new GroupImage() { QRText = "112233", GroupName = "test" }, "test_qr2", "test_qr2.jpg");

            var result = _generater.GenerateImage(null!, "test_qr3.jpg");
            Assert.That(result, Is.False);
            result = _generater.GenerateImage(new GroupImage(), "test_qr3.jpg");
            Assert.That(result, Is.False);
        }

        private void TestName(GroupImage group, string name, string realName)
        {
            var result = _generater.GenerateImage(group, name);
            Assert.That(result, Is.True);

            bool isExsit = File.Exists(Path.Combine(Folder, realName));
            Assert.That(isExsit, Is.True);
        }
    }
}
