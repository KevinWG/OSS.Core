namespace OSS.Core.Module.Portal;

internal interface IRoleCommonService: IRoleOpenService
{
    /// <summary>
    /// 通过用户id获取角色id列表
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    internal Task<List<long>> GetRoleIdsByUserId(long userId);
}