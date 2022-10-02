using HuoHuan.DataBase.Services;
using Moq;
using NUnit.Framework;

namespace HuoHuan.DataBase.Test
{
    [TestFixture]
    internal class GroupDBTest
    {
        private GroupDB _db = null!;

        [SetUp]
        public void SetUp()
        {
            var path = "demo_path";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Mock<GroupDB> mock = new();
            mock.Setup(t => t.FilePath).Returns(path);
            _db = mock.Object;

        }

        [Test]
        public void TestInsert()
        {
            Assert.That(this._db.IsExistsUrl("test").Result, Is.False);
            this._db.InsertGroup(new Models.GroupImage() { Url = "test", InvalidateDate = DateTime.Today }).Wait();
            Assert.That(this._db.IsExistsUrl("test").Result);
        }

        [Test]
        public void TestQuery()
        {
            this._db.InsertGroup(new Models.GroupImage() { Url = "test1", InvalidateDate = DateTime.Today }).Wait();
            this._db.InsertGroup(new Models.GroupImage() { Url = "test2", InvalidateDate = DateTime.Today.AddDays(-100) }).Wait();
            var result = this._db.QueryValidateGroup().Result;
            if (result is null || result.Count != 1)
            {
                Assert.Fail();
            }
            else
            {
                Assert.That(result[0]?.Url, Is.EqualTo("test1"));
            }
        }

        [Test]
        public void TestDelete()
        {
            var url = "test3";
            this._db.InsertGroup(new Models.GroupImage() { Url = url, InvalidateDate = DateTime.Today.AddDays(-100) }).Wait();
            Assert.That(this._db.IsExistsUrl(url).Result);
            this._db.DeleteUrl(url).Wait();
            Assert.That(!this._db.IsExistsUrl(url).Result);
        }
    }
}
