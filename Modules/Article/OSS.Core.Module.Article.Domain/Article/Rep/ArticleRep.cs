using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  Article 对象仓储
/// </summary>
public class ArticleRep : BaseArticleRep<ArticleMo,long> 
{
    /// <inheritdoc />
    public ArticleRep() : base("m_article")
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
