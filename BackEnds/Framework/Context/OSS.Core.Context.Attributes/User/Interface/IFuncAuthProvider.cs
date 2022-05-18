﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    /// 功能权限判断接口
    /// </summary>
    public interface IFuncAuthProvider
    {
        /// <summary>
        ///  校验功能权限
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userIdentity"></param>
        /// <param name="funcCode"></param>
        /// <returns></returns>
        Task<Resp> FuncAuthorize(HttpContext context, UserIdentity userIdentity, AskUserFunc funcCode);
    }
}
