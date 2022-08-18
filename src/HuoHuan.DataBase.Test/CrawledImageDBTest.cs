using HuoHuan.DataBase.Services;
using Moq;
using NUnit.Framework;

namespace HuoHuan.DataBase.Test
{
    [TestFixture]
    public class CrawledImageDBTest
    {
        CrawledImageDB _db = null!;

        [SetUp]
        public void Setup()
        {
            var path = "demo_path";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Mock<CrawledImageDB> mock = new();
            mock.Setup(t => t.FilePath).Returns(path);
            _db = mock.Object;
        }

        [Test]
        public void TestInsert()
        {
            Assert.That(this._db.IsExists("test_insert").Result, Is.False);
            this._db.Insert("test_insert").Wait();
            Assert.Multiple(() =>
            {
                Assert.That(this._db.IsExists("test_insert").Result);
                Assert.That(this._db.IsExistsAndInsert("test_insert").Result);
                Assert.That(this._db.IsExistsAndInsert("test_insert_2").Result, Is.False);
            });
        }

        [Test]
        public void TestClear()
        {
            this._db.Insert("test_clear1", DateTime.Now.AddDays(-60)).Wait();
            this._db.Insert("test_clear2").Wait();
            Assert.Multiple(() =>
            {
                Assert.That(this._db.IsExists("test_clear1").Result, Is.True);
                Assert.That(this._db.IsExists("test_clear2").Result, Is.True);
            });
            this._db.ClearStaleUrls().Wait();
            Assert.Multiple(() =>
            {
                Assert.That(this._db.IsExists("test_clear1").Result, Is.False);
                Assert.That(this._db.IsExists("test_clear2").Result);
            });
        }
    }
}
