#region Copyright (C) 2018 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 成员信息公用类 （前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2018-11-19
*       
*****************************************************************************/

#endregion

using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    /// 用户授权验证实现接口
    /// </summary>
    public interface IUserAuthProvider
    {
        /// <summary>
        ///  中间件初始化用户信息
        /// </summary>
        /// <returns></returns>
        Task<Resp<UserIdentity>> GetIdentity();
    }
}
