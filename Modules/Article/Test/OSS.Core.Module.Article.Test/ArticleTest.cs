using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Article.Test
{
    [TestClass]
    public class ArticleTest:BaseTest
    {
        private static readonly ArticleService _service = new ArticleService();

        [TestMethod]
        public async Task TestMethod1()
        {
            const long categoryId = 1000000000000000000;

            var addReq = new AddArticleReq
            {
                title       = "测试文章",
                author      = "张晓",
                category_id = categoryId,
                body        = "文章单元测试，测试文章模块功能是否正常。"
            };

            var addRes = await _service.Add(addReq);
            Assert.IsTrue(addRes.IsSuccess(),addRes.msg);

            var articleId = addRes.data;
            var aIdToken  = PassTokenHelper.GenerateToken(articleId.ToString());

            var updateReq = new AddArticleReq
            {
                title       = "(修改版)测试文章",
                author      = "张",
                category_id = categoryId,
                body        = "(修改版)文章单元测试，测试文章模块功能是否正常。"
            };
            var updateRes = await _service.Edit(aIdToken, updateReq);
            Assert.IsTrue(updateRes.IsSuccess());

            var searchRes = await _service.MSearch(new SearchReq()
            {
                filter = new Dictionary<string, string>() { { "category_id", categoryId.ToString() } }
            });
            Assert.IsTrue(searchRes.IsSuccess() && searchRes.data.Count > 0, searchRes.msg ?? "没有搜索到文章信息");

            var getRes = await _service.Get(articleId);
            Assert.IsTrue(getRes.IsSuccess(), getRes.msg);

            var deleteRes = await _service.Delete(aIdToken);
            Assert.IsTrue(deleteRes.IsSuccess(),deleteRes.msg);
        }
    }
}