using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  模块授权认证
    /// </summary>
    public interface IModuleAuthProvider
    {
        /// <summary>
        ///  校验模块权限
        /// </summary>
        /// <returns></returns>
        Task<IResp> Authorize();
    }
}
