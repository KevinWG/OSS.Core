﻿using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

/// <summary>
///  组织机构 仓储层接口
/// </summary>
public interface IOrganizationRep:IRepository<OrganizationMo,long>
{
    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PageList<OrganizationMo>> Search(SearchReq req);

    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    Task<Resp> UpdateStatus(long id, CommonStatus status);
}
