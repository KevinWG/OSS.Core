//using OSS.Common;
//using OSS.Common.Resp;
//using OSS.Core.Context;
//using OSS.Core.Domain;

//namespace TM.WMS;

///// <summary>
/////  物料库存单位 对象仓储
///// </summary>
//public class MCodeRep : BaseWMSRep<MSkuMo, long>
//{
//    /// <inheritdoc />
//    public MCodeRep() : base("wms_m_sku")
//    {
//    }

//    ///// <summary>
//    /////  查询列表
//    ///// </summary>
//    ///// <param name="req"></param>
//    ///// <returns></returns>
//    //public Task<PageList<MSkuMo>> Search(SearchReq req)
//    //{
//    //    return SimpleSearch(req);
//    //}


//    public Task<List<MSkuMo>> GetByMId(long mId)
//    {
//        var tenantId = CoreContext.GetTenantLongId();
//        return GetList(w => w.material_id == mId && w.tenant_id == tenantId);
//    }

//    /// <summary>
//    ///   修改状态
//    /// </summary>
//    /// <param name="id"></param>
//    /// <param name="status"></param>
//    /// <returns></returns>
//    public Task<Resp> UpdateStatus(long id, CommonStatus status)
//    {
//        var tenantId = CoreContext.GetTenantLongId();
//        return Update(u => new { u.status }, w => w.id == id && w.tenant_id == tenantId, new { status });
//    }

//    /// <summary>
//    ///  修改单位项信息
//    /// </summary>
//    /// <param name="item"></param>
//    /// <returns></returns>
//    public Task<Resp> Edit(MSkuMo item)
//    {
//        var tenantId = CoreContext.GetTenantLongId();
//        return Update(u => new { u.bar_code, u.ratio }, w => w.id == item.id && w.tenant_id == tenantId, item);
//    }
//}
