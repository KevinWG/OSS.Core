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
                name = "�з���",
                remark = "�������з���"
            });
            Assert.IsTrue(addRootRes.IsSuccess(), addRootRes.msg);

            var addSecRes = await _service.Add(new AddWarehouseReq()
            {
                name = "Ĭ���Ӳ�",
                parent_id = addRootRes.data,
                remark = "��ʱ��"
            });

            Assert.IsTrue(addSecRes.IsSuccess(), addSecRes.msg);

            var addAreaRes = await _service.AddArea(new AddAreaReq()
            {
                warehouse_id = addSecRes.data,
                code = "A10",
                remark = "ר����Ŀ��",
                //trade_flag = TradeFlag.Normal
            });
            Assert.IsTrue(addAreaRes.IsSuccess(), addAreaRes.msg);

            var listRes = await _service.AllUseable();
            Assert.IsTrue(listRes.IsSuccess(), listRes.msg);

            var areaListRes = await _service.AreaList(addSecRes.data);
            Assert.IsTrue(areaListRes.IsSuccess(), areaListRes.msg);

            var updateRes = await _service.SetUseable(addSecRes.data, 0);
            Assert.IsTrue(!updateRes.IsSuccess(), "���ڻ������������");

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