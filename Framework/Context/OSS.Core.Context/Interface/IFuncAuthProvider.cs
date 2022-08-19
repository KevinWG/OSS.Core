﻿using OSS.Common.Resp;

namespace OSS.Core.Context
{
    /// <summary>
    /// 功能权限判断接口
    /// </summary>
    public interface IFuncAuthProvider
    {
        /// <summary>
        ///  校验功能权限
        /// </summary>
        /// <param name="funcCode"></param>
        /// <returns></returns>
        Task<IResp<FuncDataLevel>> Authorize(string funcCode);
    }
}
