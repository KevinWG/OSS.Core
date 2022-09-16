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
                title       = "��������",
                author      = "����",
                category_id = categoryId,
                body        = "���µ�Ԫ���ԣ���������ģ�鹦���Ƿ�������"
            };

            var addRes = await _service.Add(addReq);
            Assert.IsTrue(addRes.IsSuccess(),addRes.msg);

            var articleId = addRes.data;
            var aIdToken  = PassTokenHelper.GenerateToken(articleId.ToString());

            var updateReq = new AddArticleReq
            {
                title       = "(�޸İ�)��������",
                author      = "��",
                category_id = categoryId,
                body        = "(�޸İ�)���µ�Ԫ���ԣ���������ģ�鹦���Ƿ�������"
            };
            var updateRes = await _service.Edit(aIdToken, updateReq);
            Assert.IsTrue(updateRes.IsSuccess());

            var searchRes = await _service.MSearch(new SearchReq()
            {
                filter = new Dictionary<string, string>() { { "category_id", categoryId.ToString() } }
            });
            Assert.IsTrue(searchRes.IsSuccess() && searchRes.data.Count > 0, searchRes.msg ?? "û��������������Ϣ");

            var getRes = await _service.Get(articleId);
            Assert.IsTrue(getRes.IsSuccess(), getRes.msg);

            var deleteRes = await _service.Delete(aIdToken);
            Assert.IsTrue(deleteRes.IsSuccess(),deleteRes.msg);
        }
    }
}