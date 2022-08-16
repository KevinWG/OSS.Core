﻿using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.WorkFlow;

public interface IOpenedNodeService
{
    /// <summary>
    ///  查询列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<NodeMo>> Search(SearchReq req);

    /// <summary>
    ///  通过id获取模板信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IResp<NodeMo>> Get(long id);

    /// <summary>
    ///  设置可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<IResp> SetUseable(long id, ushort flag);

    /// <summary>
    ///  添加对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp> Add(AddNodeReq req);
}