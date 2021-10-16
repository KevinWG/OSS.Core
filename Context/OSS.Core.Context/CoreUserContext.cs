#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：用户基础信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

using System;
using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Core.Context.Helper;

namespace OSS.Core.Context
{
    /// <summary>
    /// 当前系统访问上下文信息
    /// </summary>
    public static class CoreUserContext
    {
        /// <summary>
        /// 是否已经验证
        /// </summary>
        public static bool IsAuthenticated => Identity != null;

        #region  成员信息

        /// <summary>
        ///   成员身份信息
        /// </summary>
        public static UserIdentity Identity
            => ContextHelper.GetContext()?.MemberIdentity;

        /// <summary>
        ///   设置用户信息
        /// </summary>
        /// <param name="info"></param>
        public static void SetIdentity(UserIdentity info)
        {
            ContextHelper.SetMemberIdentity(info);
        }

        ///// <summary>
        ///// 获取成员扩展详情
        ///// </summary>
        ///// <typeparam name="TMInfo"></typeparam>
        ///// <returns></returns>
        //public static TMInfo GetMemberInfo<TMInfo>()
        //    where TMInfo : class => Identity?.MemberInfo as TMInfo;

        #endregion

        #region    token  处理

        /// <summary>
        /// 通过 ID 生成对应的Token
        /// </summary>
        /// <param name="encryptKey"></param>
        /// <param name="tokenDetail"></param>
        /// <returns></returns>
        public static string GetToken(string encryptKey, string tokenDetail)
        {
            return AesRijndael.Encrypt(tokenDetail, encryptKey).ReplaceBase64ToUrlSafe();
        }

        /// <summary>
        ///  通过token解析出对应的id和key
        /// </summary>
        /// <param name="encryptKey"></param>
        /// <param name="token"></param>
        /// <returns>返回解析信息，Item1为id，Item2为key</returns>
        public static string GetTokenDetail(string encryptKey, string token)
        {
            var tokenDetail = AesRijndael.Decrypt(token.ReplaceBase64UrlSafeBack(), encryptKey);

            if (string.IsNullOrEmpty(tokenDetail))
                throw new ArgumentNullException(nameof(token), "不合法的用户Token");

            return tokenDetail;
        }

        #endregion
    }


}