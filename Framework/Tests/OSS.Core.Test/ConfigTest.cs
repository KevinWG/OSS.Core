using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Tools.DirConfig;

namespace OSS.Core.Test
{
    [TestClass]
    public class ConfigTest:BaseTest
    {
       
        [TestMethod]
        public async Task TestMethod1()
        {
            var res= await ListConfigHelper.SetItem("test_list_key", "test_item_key",
                new TestConfig()
                {
                    name = "test"
                });
            Assert.IsTrue(res);

            var item =await ListConfigHelper.GetItem<TestConfig>("test_list_key", "test_item_key");
            Assert.IsNotNull(item);

            var list = await ListConfigHelper.GetList<TestConfig>("test_list_key");
            Assert.IsNotNull(list.Count>0);


            var count = await ListConfigHelper.GetCount("test_list_key");
            Assert.IsNotNull(count > 0);
        }
    }


    public class TestConfig
    {
        public string name { get; set; }
    }
}