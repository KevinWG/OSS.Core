using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace TM.Module.Product;

/// <summary>
///  ProductCategory 服务
/// </summary>
public class SpuCategoryService : ISpuCategoryOpenService
{
    private static readonly SpuCategoryRep _categoryRep = new();

    /// <inheritdoc />
    public async Task<PageListResp<SpuCategoryMo>> MSearch(SearchReq req)
    {
        var pageList = await _categoryRep.Search(req);
        return new PageListResp<SpuCategoryMo>(pageList);
    }

    /// <inheritdoc />
    public async Task<ListResp<SpuCategoryTreeItem>> AllCategories()
    {
        var req = new SearchReq()
        {
            size = 2000
        };
        req.filter.Add("status","0");
        var pList =  await _categoryRep.Search(req);

        var treeList = pList.data.ToIndent<SpuCategoryMo,SpuCategoryTreeItem,long>((o, child) =>
        {
            var newItem = o.ToTreeItem();
            newItem.children = child;
            return newItem;
        }, c => c.parent_id, c => c.id, 0);

        return new ListResp<SpuCategoryTreeItem>(treeList);
    }

    /// <inheritdoc />
    public async Task<Resp<SpuCategoryMo>> Get(long id)
    {
        var getRes = await _categoryRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<SpuCategoryMo>().WithResp(getRes, "未能找到目录信息");
    }


    /// <inheritdoc />
    public async Task<Resp<SpuCategoryMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;

        return getRes.data.status >= 0
            ? getRes
            : new Resp<SpuCategoryMo>().WithResp(RespCodes.OperateObjectNull, "未能找到有效可用的目录信息");
    }


    /// <inheritdoc />
    public async Task<LongResp> Add(AddSpuCategoryReq req)
    {
        var c = new SpuCategoryMo();
        if (req.parent_id > 0)
        {
            var parentCategoryRes = await GetUseable(req.parent_id);
            if (!parentCategoryRes.IsSuccess())
                return new LongResp().WithResp(parentCategoryRes);

            var parent = parentCategoryRes.data;

            c.level = parent.level + 1;
        }

        if (c.level > 4)
            return new LongResp().WithResp(RespCodes.OperateFailed, "分类层级不能超过五层!");

        var newIdRes = await GetNewCategoryId(req.parent_id);
        if (!newIdRes.IsSuccess())
            return newIdRes;

        c.id = newIdRes.data;
        c.name = req.name;
        c.parent_id = req.parent_id;
        c.order = req.order;

        c.FormatBaseByContext();

        await _categoryRep.Add(c);
        return new LongResp(c.id);
    }

    private static async Task<LongResp> GetNewCategoryId(long parentId)
    {
        var maxNumRes = await _categoryRep.GetLastSubNum(parentId);

        if (!maxNumRes.IsSuccess())
            return new LongResp().WithResp(RespCodes.OperateFailed, "查询目录树形编号出现错误!");

        // 生成树形编码
        var maxSubId = maxNumRes.data;
        long treeNum;
        try
        {
            treeNum = TreeNumHelper.GenerateSmallNum(parentId, maxSubId);
        }
        catch (Exception)
        {
            return new LongResp().WithResp(RespCodes.OperateFailed, "无法生成有效的目录编码，可能已经超出长度限制");
        }

        return new LongResp(treeNum);
    }

    /// <inheritdoc />
    public async Task<Resp> SetUseable(long id, ushort flag)
    {
        var status = flag == 1 ? CommonStatus.Original : CommonStatus.UnActive;

        if (status == CommonStatus.UnActive)
        {
            var countRes = await _categoryRep.GetCountByParent(id);
            if (!countRes.IsSuccess())
                return countRes;

            if (countRes.data > 0)
                return new Resp(RespCodes.OperateFailed, "存在子级目录目录，不可直接作废！");
        }

        return await _categoryRep.UpdateStatus(id, status);
    }

    /// <inheritdoc />
    public Task<Resp> UpdateName(long id, UpdateSCNameReq req)
    {
        return _categoryRep.UpdateName(id, req);
    }

    /// <summary>
    /// 修改排序
    /// </summary>
    /// <param name="id"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    public Task<Resp> UpdateOrder(long id, int order)
    {
        return _categoryRep.UpdateOrder(id, order);
    }
}