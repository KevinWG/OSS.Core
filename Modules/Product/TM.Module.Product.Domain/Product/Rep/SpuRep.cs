using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace TM.Module.Product;

/// <summary>
///  产品 对象仓储
/// </summary>
public class SpuRep : BaseProductRep<SpuMo, long>
{
    /// <inheritdoc />
    public SpuRep() : base("product_spu")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<SpuMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }

    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<Resp> UpdateStatus(long id, CommonStatus status)
    {
        return Update(u => new {u.status}, w => w.id == id, new {status});
    }
}
