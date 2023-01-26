using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

public interface IFuncRep : IRepository<FuncMo, long>
{
    Task<List<FuncMo>> GetAllFuncItems();

    /// <summary>
    ///  通过权限编码获取权限信息
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<Resp<FuncMo>> GetByCode(string code);

    /// <summary>
    ///  通过code更新权限信息
    /// </summary>
    /// <param name="code"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> UpdateByCode(string code, ChangeFuncItemReq req);

    /// <summary>
    ///  修改状态
    /// </summary>
    /// <param name="code"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    Task<Resp> UpdateStatus(string code, CommonStatus status);
}