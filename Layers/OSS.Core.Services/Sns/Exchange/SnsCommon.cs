
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 社交模块通用/对外方法
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-21
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComUtils;
using OSS.Common.Extention;
using OSS.Core.Domains.Sns.Interfaces;
using OSS.Core.Domains.Sns.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Sns.Oauth.Handlers;

namespace OSS.Core.Services.Sns.Exchange
{
    /// <summary>
    ///  社交模块通用/对外方法
    /// </summary>
    internal  static class SnsCommon
    {
        #region  Oauth模块

        /// <summary>
        ///  获取授权用户并更新信息
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal static async Task<ResultMo<OauthUserMo>> GetOauthUserByCode(SocialPaltforms plat, string code, string state)
        {
            var handler = GetHandlerByPlatform(plat);
            var tokenRes = await handler.GetOauthTokenAsync(code, state);
            if (!tokenRes.IsSuccess())
                return tokenRes.ConvertToResultOnly<OauthUserMo>();
            
            var userWxRes = await handler.GetOauthUserAsync(tokenRes.data.access_token, tokenRes.data.app_user_id);
            if (!userWxRes.IsSuccess())
                return tokenRes.ConvertToResultOnly<OauthUserMo>();

            var userRes = await InsContainer<IOauthUserRep>.Instance.GetOauthUserByAppUId(MemberShiper.AppAuthorize.TenantId.ToInt64(),
                tokenRes.data.app_user_id,plat);
            if (userRes.IsSuccess())
            {
                var user = userRes.data;
                user.ResetFromSocial(userWxRes.data);
                user.SetTokenInfo(tokenRes.data);

                await InsContainer<IOauthUserRep>.Instance.UpdateUserWithToken(user);
                return new ResultMo<OauthUserMo>(user);
            }
            else
            {
                var user = userWxRes.data;
                user.SetTokenInfo(tokenRes.data);

                var idRes=await InsContainer<IOauthUserRep>.Instance.Insert(user);
                if (!idRes.IsSuccess())
                    return idRes.ConvertToResultOnly<OauthUserMo>();

                user.Id = idRes.id;
                return new ResultMo<OauthUserMo>(user);
            }

        }

        /// <summary>
        /// 获取处理Hander
        /// </summary>
        /// <param name="plat">平台类型</param>
        /// <returns></returns>
        internal static IOauthHander GetHandlerByPlatform(SocialPaltforms plat)
        {
            IOauthHander handler;
            switch (plat)
            {
                case SocialPaltforms.Wechat:
                    handler = WxOauthHander.Instance;
                    break;
                //  todo 添加其他平台
                default:
                    handler = BaseOauthHander<NoneOauthHander>.Instance;
                    break;
            }

            handler.SetCOntextConfig(MemberShiper.AppAuthorize);
            return handler;
        }

        #endregion
    }
}
