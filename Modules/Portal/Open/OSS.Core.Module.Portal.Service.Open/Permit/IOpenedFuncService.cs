using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public interface IOpenedFuncService
{
    /// <summary>
    /// 检查是否已经存在FuncCode
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<IResp> CheckFuncCode(string code);

    /// <summary>
    ///  获取系统所有权限项
    /// </summary>
    /// <returns></returns>
    Task<ListResp<FuncMo>> GetAllFuncItems();

    /// <summary>
    ///  添加权限码
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> AddFuncItem(AddFuncItemReq req);

    /// <summary>
    ///  修改权限信息
    /// </summary>
    /// <param name="code"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> ChangeFuncItem(string code, ChangeFuncItemReq req);

    ///// <summary>
    ///// 设置权限项可用状态
    ///// </summary>
    ///// <returns></returns>
    //Task<IResp> SetUseable(string code, ushort flag);
}