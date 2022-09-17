using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  文章专题关联 对象仓储
/// </summary>
public class ArticleTopicRep : BaseArticleRep<ArticleTopicMo,long> 
{
    /// <inheritdoc />
    public ArticleTopicRep() : base(ArticleConst.RepTables.ArticleTopic)
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<ArticleTopicMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
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
}
