using NUnit.Framework;
using System;
using System.Reflection;

namespace HuoHuan.Test
{
    [TestFixture]
    internal class FilterTest
    {
        [Test]
        public void TestFactory()
        {
            Type type = Assembly.Load("HuoHuan").GetType("HuoHuan.Core.Filter.GroupFilterFactory")!;

            if (type == null)
            {
                Assert.Fail();
            }
            else
            {
                var methods = type.GetMethods();
                var instance = methods[0].Invoke(null, new object[] { 1 });

                if (instance?.GetType().Name.Equals("WechatGroupFilter") == true)
                {
                    Assert.Pass(); 
                }
                else
                { 
                    Assert.Fail();
                }
            }
        }
    }
}
