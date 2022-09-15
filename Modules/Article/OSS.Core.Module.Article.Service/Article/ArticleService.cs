using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  Article 服务
/// </summary>
public class ArticleService : IArticleOpenService
{
    private static readonly ArticleRep _ArticleRep = new();

    /// <inheritdoc />
    public async Task<PageListResp<ArticleMo>> Search(SearchReq req)
    {
        return new PageListResp<ArticleMo>(await _ArticleRep.Search(req));
    }

    /// <inheritdoc />
    public async Task<IResp<ArticleMo>> Get(long id)
    {
        var  getRes = await _ArticleRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<ArticleMo>().WithErrMsg("未能找到文章信息");
    }

    /// <inheritdoc />
    public async Task<IResp<ArticleMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        return getRes.IsSuccess() && getRes.data.status >= 0 ? getRes : new Resp<ArticleMo>().WithErrMsg("未能找到有效的文章信息");
    }

    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _ArticleRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<IResp> Add(AddArticleReq req)
    {
        // 判断当前分类是否正常
        var categoryRes = await InsContainer<ICategoryService>.Instance.GetUseable(req.category_id);
        if (categoryRes.IsSuccess())
            return categoryRes;

        var mo = req.MapToArticleMo();

        await _ArticleRep.Add(mo);
        return Resp.DefaultSuccess;
    }
}
