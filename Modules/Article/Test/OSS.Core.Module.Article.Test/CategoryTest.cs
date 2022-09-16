using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Article.Test
{
    [TestClass]
    public class CategoryTest : BaseTest
    {
        private static readonly CategoryService _categoryService = new ();

        [TestMethod]
        public async Task Test()
        {
            var addRes = await _categoryService.Add(new AddCategoryReq()
            {
                name      = "测试分类",
                parent_id = 0
            });

            var cId = addRes.data;

            Assert.IsTrue(addRes.IsSuccess(),addRes.msg);

            var usableCRes = await _categoryService.GetUseable(cId);
            Assert.IsTrue(usableCRes.IsSuccess(), usableCRes.msg);

            var setRes = await _categoryService.SetUseable(cId, 0);
            Assert.IsTrue(setRes.IsSuccess(), setRes.msg);

            usableCRes = await _categoryService.GetUseable(cId);
            Assert.IsTrue(!usableCRes.IsSuccess(),"作废分类不应该通过可用接口获取到");

            var cRes = await _categoryService.Get(cId);
            Assert.IsTrue(cRes.IsSuccess(), cRes.msg);
            
            setRes = await _categoryService.SetUseable(cId, 1);
            Assert.IsTrue(setRes.IsSuccess(), setRes.msg);

            var addSubRes = await _categoryService.Add(new AddCategoryReq()
            {
                name      = "测试子分类",
                parent_id = cId
            });

            Assert.IsTrue(addSubRes.IsSuccess(), addSubRes.msg);

            //var subId = addSubRes.data;

            var searchRes = await _categoryService.Search(new SearchReq()
            {
                filter = new Dictionary<string, string>(){{"parent_id", cId.ToString() } }
            });
            Assert.IsTrue(searchRes.IsSuccess() && searchRes.data.Count>0, searchRes.msg);

        }
    }
}