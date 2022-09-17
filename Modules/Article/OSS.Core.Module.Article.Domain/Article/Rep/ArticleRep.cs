using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  Article 对象仓储
/// </summary>
public class ArticleRep : BaseArticleRep<ArticleMo,long> 
{
    /// <inheritdoc />
    public ArticleRep() : base(ArticleConst.RepTables.Article)
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<ArticleMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }


    /// <inheritdoc />
    protected override string BuildSimpleSearch_FilterItemSql(string key, string value, Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "category_id":
                sqlParas.Add("@category_id",value.ToInt64());
                return " t.category_id=@category_id ";
        }
        return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
    }

    /// <inheritdoc />
    protected override string BuildSimpleSearch_TableName(SearchReq req, Dictionary<string, object> sqlParas)
    {

        if (req.filter.ContainsKey("topic_id"))
        {
            var topicId = req.filter["topic_id"].ToInt64();
            return
                $"  {ArticleConst.RepTables.Article} t inner join {ArticleConst.RepTables.ArticleTopic} at on at.topic_id={topicId} and t.id = at.article_id ";
        }
        return base.BuildSimpleSearch_TableName(req, sqlParas);
    }


    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<IResp> UpdateStatus(long id, CommonStatus status)
    {
        return Update(u => new {u.status}, w => w.id == id, new {status});
    }

    public Task<IResp> Edit(long id, AddArticleReq req)
    {
        return Update(a => new
        {
            a.title,
            a.author,
            a.head_img,
            a.brief,
            a.category_id,
            a.body,
            a.attaches,
            a.tags
        },w=>w.id==id, req);
    }
}
