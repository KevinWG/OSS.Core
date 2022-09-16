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
        return getRes.IsSuccess() ? getRes : new Resp<CategoryMo>().WithResp(getRes,"未能找到分类信息");
    }

    /// <inheritdoc />
    public async Task<IResp<CategoryMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;
        
        return getRes.data.status >= 0 ? getRes : new Resp<CategoryMo>().WithResp(RespCodes.OperateObjectNull,"未能找到有效可用的分类信息");
    }

    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _CategoryRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddCategoryReq req)
    {
        var mo = req.MapToCategoryMo();

        if (req.parent_id>0)
        {
            var parentRes = await GetUseable(req.parent_id);
            if (!parentRes.IsSuccess())
                return new LongResp().WithResp(parentRes,"所属父级分类异常!");

            mo.deep_level = parentRes.data.deep_level + 1;
        }
        
        await _CategoryRep.Add(mo);
        return new LongResp(mo.id);
    }
}


internal interface ICategoryService : ICategoryOpenService
{

}
