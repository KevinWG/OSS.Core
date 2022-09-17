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
            Assert.IsTrue(addRes.IsSuccess(), addRes.msg);

            var articleId = addRes.data;

            var relateRes = await _service.RelateTopics(new RelateTopicReq()
            {
                article_id = articleId,topics = new List<long>(){1,2}
            });
            Assert.IsTrue(relateRes.IsSuccess(), relateRes.msg);


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
                filter = new Dictionary<string, string>() { { "category_id", categoryId.ToString() } , { "topic_id", "1" } }
            });
            Assert.IsTrue(searchRes.IsSuccess() && searchRes.data.Count > 0, searchRes.msg ?? "û��������������Ϣ");

            var getRes = await _service.Get(articleId);
            Assert.IsTrue(getRes.IsSuccess(), getRes.msg);

            var deleteRes = await _service.Delete(aIdToken);
            Assert.IsTrue(deleteRes.IsSuccess(),deleteRes.msg);
        }


        private static readonly TopicService _topicService = new();

        [TestMethod]
        public async Task TestTopic()
        {
            var addRes = await _topicService.Add(new AddTopicReq()
            {
                name  = "����ר��",
                brief = "��ֻ��һ�������õ�ר��"
            });

            var tId = addRes.data;

            Assert.IsTrue(addRes.IsSuccess(), addRes.msg);

            var usableTRes = await _topicService.GetUseable(tId);
            Assert.IsTrue(usableTRes.IsSuccess(), usableTRes.msg);


            var tIdToken = PassTokenHelper.GenerateToken(tId.ToString());

            var setRes = await _topicService.SetUseable(tIdToken, 0);
            Assert.IsTrue(setRes.IsSuccess(), setRes.msg);

            usableTRes = await _topicService.GetUseable(tId);
            Assert.IsTrue(!usableTRes.IsSuccess(), "����ר�ⲻӦ��ͨ�����ýӿڻ�ȡ��");

            var cRes = await _topicService.Get(tId);
            Assert.IsTrue(cRes.IsSuccess(), cRes.msg);

            var searchRes = await _topicService.MSearch(new SearchReq());
            Assert.IsTrue(searchRes.IsSuccess() && searchRes.data.Count > 0, searchRes.msg);

        }
    }
}