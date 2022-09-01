using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public class FuncService : IFuncCommonService
{
    #region 权限码处理

    private static readonly IFuncRep _funcRep = InsContainer<IFuncRep>.Instance;

    /// <summary>
    /// 检查是否已经存在FuncCode
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<IResp> CheckFuncCode(string code)
    {
        var funcRes = await _funcRep.GetByCode(code);
        if (funcRes.IsSuccess())
            return new Resp(RespCodes.OperateObjectExisted, "权限编码已存在!");

        return funcRes.IsObjectNull() ? Resp.DefaultSuccess : funcRes;
    }

    /// <summary>
    ///  获取系统所有权限项
    /// </summary>
    /// <returns></returns>
    public  async Task<ListResp<FuncMo>> GetAllFuncItems()
    {
        return new ListResp<FuncMo>(await _funcRep.GetAllFuncItems());
    }

    /// <summary>
    ///  添加权限码
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<IResp> AddFuncItem(AddFuncItemReq req)
    {
        var checkRes = await CheckFuncCode(req.code);
        if (!checkRes.IsSuccess())
            return checkRes;

        if (!string.IsNullOrEmpty(req.parent_code))
        {
            var parentRes = await _funcRep.GetByCode(req.parent_code);
            if (!parentRes.IsSuccess())
                return new Resp().WithResp(parentRes, "未能查询到有效父级权限！");
        }

        var mo = req.ToMo();
        await _funcRep.Add(mo);

        return Resp.DefaultSuccess;
    }

    /// <summary>
    ///  修改权限信息
    /// </summary>
    /// <param name="code"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<IResp> ChangeFuncItem(string code, ChangeFuncItemReq req)
    {
        var funcRes = await _funcRep.GetByCode(code);
        if (!funcRes.IsSuccess())
            return new Resp().WithResp(funcRes, "未能查询到有效权限信息！");

        //if (!string.IsNullOrEmpty(req.parent_code))
        //{
        //    var parentRes = await _funcRep.GetByCode(req.parent_code);
        //    if (!parentRes.IsSuccess())
        //        return new Resp().WithResp(parentRes, "未能查询到有效父级权限！");
        //}

        return await _funcRep.UpdateByCode(code, req);
    }

    ///// <inheritdoc />
    //public Task<IResp> SetUseable(string code,ushort flag )
    //{
    //    return _funcRep.UpdateStatus(code, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    //}

    #endregion

}