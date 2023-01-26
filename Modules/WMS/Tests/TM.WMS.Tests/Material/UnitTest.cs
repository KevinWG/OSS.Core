using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS.Tests
{
    [TestClass]
    public class UnitTest:BaseTest
    {
        private static readonly UnitService _service = new UnitService();

        [TestMethod]
        public async Task TestMethod1()
        {
            var addRes = await _service.Add(new AddUnitReq()
            {
                name = "¸ö",
            });

            var allRes = await _service.All();
            Assert.IsTrue(allRes.IsSuccess(), allRes.msg);
        }
    }
}