using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Tools.Cache;

namespace OSS.Core.Module.Portal;

public class FuncRep : BasePortalRep<FuncMo>, IFuncRep
{
    public FuncRep() : base("b_permit_func")
    {
    }

    public Task<List<FuncMo>> GetAllFuncItems()
    {
        var getListFunc = () => GetList(w => w.status == CommonStatus.Original);
        return getListFunc.WithAbsoluteCacheAsync(PortalConst.CacheKeys.Permit_Func_All, TimeSpan.FromMinutes(30));
    }

    public override async Task Add(FuncMo mo)
    {
        await base.Add(mo);
        await CacheHelper.RemoveAsync(PortalConst.CacheKeys.Permit_Func_All);
    }

    /// <inheritdoc />
    public Task<Resp<FuncMo>> GetByCode(string code)
    {
        return Get(w => w.code == code && w.status > CommonStatus.Deleted);
    }

    /// <inheritdoc />
    public Task<Resp> UpdateByCode(string code, ChangeFuncItemReq req)
    {
        return Update(u => new
            {
                u.title
            }, w => w.code == code, req)
            .WithRespCacheClearAsync(PortalConst.CacheKeys.Permit_Func_All);
    }

    public Task<Resp> UpdateStatus(string code, CommonStatus status)
    {
        return Update(u => new
            {
                u.status
            }, w => w.code == code, new{ status })
            .WithRespCacheClearAsync(PortalConst.CacheKeys.Permit_Func_All);
    }

}