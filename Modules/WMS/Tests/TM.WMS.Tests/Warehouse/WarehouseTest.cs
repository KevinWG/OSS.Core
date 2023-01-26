using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS.Tests
{
    [TestClass]
    public class WarehouseTest : BaseTest
    {
        private static readonly IWarehouseOpenService _service = new WarehouseService();
        
        [TestMethod]
        public async Task TestMethod1()
        {
            var addRootRes = await _service.Add(new AddWarehouseReq()
            {
                name = "研发仓",
                remark = "生产部研发部"
            });
            Assert.IsTrue(addRootRes.IsSuccess(), addRootRes.msg);

            var addSecRes = await _service.Add(new AddWarehouseReq()
            {
                name = "默认子仓",
                parent_id = addRootRes.data,
                remark = "临时仓"
            });

            Assert.IsTrue(addSecRes.IsSuccess(), addSecRes.msg);

            var addAreaRes = await _service.AddArea(new AddAreaReq()
            {
                warehouse_id = addSecRes.data,
                code = "A10",
                remark = "专供项目区",
                //trade_flag = TradeFlag.Normal
            });
            Assert.IsTrue(addAreaRes.IsSuccess(), addAreaRes.msg);

            var listRes = await _service.AllUseable();
            Assert.IsTrue(listRes.IsSuccess(), listRes.msg);

            var areaListRes = await _service.AreaList(addSecRes.data);
            Assert.IsTrue(areaListRes.IsSuccess(), areaListRes.msg);

            var updateRes = await _service.SetUseable(addSecRes.data, 0);
            Assert.IsTrue(!updateRes.IsSuccess(), "存在活动库区不可作废");

            updateRes = await _service.SetAreaUseable(addAreaRes.data, 0);
            Assert.IsTrue(updateRes.IsSuccess(), updateRes.msg);

            updateRes = await _service.SetAreaTradeFlag(addAreaRes.data, TradeFlag.UnActive);
            Assert.IsTrue(updateRes.IsSuccess(), updateRes.msg);
            
            updateRes = await _service.SetUseable(addSecRes.data, 0);
            Assert.IsTrue(updateRes.IsSuccess(), updateRes.msg);

            updateRes = await _service.SetUseable(addRootRes.data, 0);
            Assert.IsTrue(updateRes.IsSuccess(), updateRes.msg);
        }
    }
}