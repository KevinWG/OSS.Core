using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  Category 服务
/// </summary>
public class CategoryService : ICategoryService
{
    private static readonly CategoryRep _CategoryRep = new();

    /// <inheritdoc />
    public async Task<PageListResp<CategoryMo>> Search(SearchReq req)
    {
        return new PageListResp<CategoryMo>(await _CategoryRep.Search(req));
    }

    /// <inheritdoc />
    public async Task<IResp<CategoryMo>> Get(long id)
    {
        var getRes = await _CategoryRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<CategoryMo>().WithErrMsg("未能找到分类信息");
    }


    /// <inheritdoc />
    public async Task<IResp<CategoryMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        return getRes.IsSuccess() && getRes.data.status >= 0 ? getRes : new Resp<CategoryMo>().WithErrMsg("未能找到有效的分类信息");
    }


    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _CategoryRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<IResp> Add(AddCategoryReq req)
    {
        var mo = req.MapToCategoryMo();
        mo.FormatBaseByContext();

        await _CategoryRep.Add(mo);
        return Resp.DefaultSuccess;
    }
}


internal interface ICategoryService : ICategoryOpenService
{

}
