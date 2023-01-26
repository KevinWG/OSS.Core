using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS.Tests
{
    [TestClass]
    public class MCategoryTest:BaseTest
    {
        private static readonly MCategoryService _service = new MCategoryService();

        [TestMethod]
        public async Task TestMethod1()
        {
            var addRes = await _service.Add(new AddCategoryReq()
            {
                parent_id = 0,
                name = "总分类",
                order = 100
            });
            Assert.IsTrue(addRes.IsSuccess(),addRes.msg);

            var cId = addRes.data;
            var updateRes = await _service.UpdateName(cId, new UpdateMCategoryNameReq()
            {
                name = "总分类（修改）"
            });
            Assert.IsTrue(updateRes.IsSuccess(), updateRes.msg);

            updateRes = await _service.UpdateOrder(cId,101);
            Assert.IsTrue(updateRes.IsSuccess(), updateRes.msg);

            addRes = await _service.Add(new AddCategoryReq()
            {
                parent_id = cId,
                name      = "总分类-子类",
                order     = 100
            });
            Assert.IsTrue(addRes.IsSuccess(), addRes.msg);

            var subCId = addRes.data;

            updateRes = await _service.SetUseable(subCId, 0);
            Assert.IsTrue(updateRes.IsSuccess(), updateRes.msg);

            updateRes = await _service.SetUseable(cId, 0);
            Assert.IsTrue(updateRes.IsSuccess(), updateRes.msg);

            //updateRes = await _service.GetUseable(cId);
            //Assert.IsTrue(!updateRes.IsSuccess(),"本该无效的类目依然获取到!");

            //var searchRes = await _service.MSearch(new SearchReq());
            //Assert.IsTrue(searchRes.IsSuccess(), searchRes.msg);
        }
    }
}