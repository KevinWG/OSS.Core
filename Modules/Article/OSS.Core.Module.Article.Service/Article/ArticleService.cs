using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;

namespace OSS.Core.Module.Article;

/// <summary>
///  Article 服务
/// </summary>
public class ArticleService : IArticleOpenService
{
    private static readonly ArticleRep _ArticleRep = new();

    /// <inheritdoc />
    public async Task<TokenPageListResp<ArticleMo>> MSearch(SearchReq req)
    {
        var pageList = await _ArticleRep.Search(req);

        return pageList.ToTokenPageRespWithIdToken();
    }

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
    public Task<IResp> Delete(string pass_token)
    {
        var id = PassTokenHelper.GetData(pass_token).ToInt64();

        return _ArticleRep.SoftDeleteById(id);
    }

    /// <inheritdoc />
    public Task<IResp> Edit(string pass_token, AddArticleReq req)
    {
        var id = PassTokenHelper.GetData(pass_token).ToInt64();
        return _ArticleRep.Edit(id, req);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddArticleReq req)
    {
        // 无需判断分类，暂时隐藏
        // var categoryRes =  await InsContainer<ICategoryService>.Instance.GetUseable(req.category_id);
        // if (categoryRes.IsSuccess())
        //    return categoryRes;

        var mo = req.MapToArticleMo();

        await _ArticleRep.Add(mo);
        return new LongResp(mo.id);
    }

  
}
