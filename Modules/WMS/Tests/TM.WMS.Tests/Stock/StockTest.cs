using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS.Tests
{
    [TestClass]
    public class StockTest : BaseTest
    {
        private static readonly IStockOpenService _service = new StockService();

        private static readonly IMaterialOpenService _mService = new MaterialService();

        private static readonly IWarehouseOpenService _wService = new WarehouseService();

        [TestMethod]
        public async Task TestMethod1()
        {
            var sReq  = new SearchReq();
            sReq.filter.Add("code", "test_material");

            var maRes = await _mService.MSearch(sReq);
            Assert.IsTrue(maRes.IsSuccess()&&maRes.data.Count>0,"未能找到测试物料");

            var material = maRes.data[0];

            var wListRes = await _wService.AllUseable();
            Assert.IsTrue(wListRes.IsSuccess()&&wListRes.data.Count>0, wListRes.msg);

            var warehouse = wListRes.data[0];

            var areaListRes = await _wService.AreaList(warehouse.id);
            Assert.IsTrue(areaListRes.IsSuccess(), areaListRes.msg);

            var wareArea = areaListRes.data.FirstOrDefault(a=>a.code== "Z0");
            if (wareArea==null)
            {
                var addAreaRes = await _wService.AddArea(new AddAreaReq()
                {
                    warehouse_id = warehouse.id,
                    code         = "Z0",
                    remark       = "测试专用"
                });
                Assert.IsTrue(addAreaRes.IsSuccess(), addAreaRes.msg);

                areaListRes = await _wService.AreaList(warehouse.id);
                Assert.IsTrue(areaListRes.IsSuccess(), areaListRes.msg);

                wareArea = areaListRes.data.FirstOrDefault(a => a.code == "Z0");
                Assert.IsNotNull(wareArea,"未能有效创建测试库区");
            }

            var changeRecord = new StockChangeReq()
            {
                material_id = material.id,
                area_id = wareArea.id,
                
                change_unit = material.basic_unit,
                change_count = 100,

                b_type = StockBusinessType.Initial,
                b_id = 1,
            };

            var changeRes = await _service.StockChange(changeRecord);
            Assert.IsTrue(changeRes.IsSuccess(), changeRes.msg);

            var searchReq  = new SearchReq();
            searchReq.filter.Add("code",material.code);

            var unionGroupRes = await _service.MSearchUnion(searchReq);
            Assert.IsTrue(unionGroupRes.IsSuccess(), unionGroupRes.msg);
        }
    }
}