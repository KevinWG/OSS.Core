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
    public async Task<Resp<CategoryMo>> Get(long id)
    {
        var getRes = await _CategoryRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<CategoryMo>().WithResp(getRes,"未能找到分类信息");
    }

    /// <inheritdoc />
    public async Task<Resp<CategoryMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;
        
        return getRes.data.status >= 0 ? getRes : new Resp<CategoryMo>().WithResp(RespCodes.OperateObjectNull,"未能找到有效可用的分类信息");
    }

    /// <inheritdoc />
    public Task<Resp> SetUseable(long id, ushort flag)
    {
        return _CategoryRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddArticleCategoryReq req)
    {
        var mo = req.MapToCategoryMo();

        if (req.parent_id>0)
        {
            var parentRes = await GetUseable(req.parent_id);
            if (!parentRes.IsSuccess())
                return new LongResp().WithResp(parentRes,"所属父级分类异常!");

            var parent = parentRes.data;

            var parentExt = string.IsNullOrEmpty(parent.parent_ext) ? string.Empty : (parent.parent_ext + ",");

            mo.parent_ext = string.Concat(parentExt, parent.id, "|", parent.name);
            mo.deep_level = parentRes.data.deep_level + 1;
        }

        var idRes = await GetNewId(req.parent_id);
        if (!idRes.IsSuccess())
            return idRes;
        
        mo.id = idRes.data;

        await _CategoryRep.Add(mo);
        return idRes;
    }

    private static async Task<LongResp> GetNewId(long parentId)
    {
        var lastSubIdRes = await _CategoryRep.GetLastSubId(parentId);
        if (!lastSubIdRes.IsSuccess() && !lastSubIdRes.IsRespCode(RespCodes.OperateObjectNull))
            return new LongResp().WithResp(lastSubIdRes);

        // 生成树形编码
        var  maxSubId = lastSubIdRes.data;
        long treeNum;
        try
        {
            treeNum = TreeNumHelper.GenerateNum(parentId, maxSubId);
        }
        catch (Exception)
        {
            return new LongResp().WithResp(RespCodes.OperateFailed, "无法生成有效的编码，可能已经超出长度限制");
        }
        return new LongResp(treeNum);
    }
}


internal interface ICategoryService : ICategoryOpenService
{

}
