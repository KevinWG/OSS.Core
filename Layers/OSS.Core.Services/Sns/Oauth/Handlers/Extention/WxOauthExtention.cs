
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 授权实体扩展转化方法实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-14
*       
*****************************************************************************/

#endregion


using System;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.Extention;
using OSS.Core.Domains.Sns.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.SnsSdk.Oauth.Wx.Mos;

namespace OSS.Core.Services.Sns.Oauth.Handlers.Extention
{
    /// <summary>
    /// 微信sdk授权信息实体的 扩展方法
    /// </summary>
    public static class WxOauthAccessTokenExtention
    {
        /// <summary>
        ///  转化微信的授权token实体到通用实体
        /// </summary>
        /// <param name="wxMo"></param>
        /// <returns></returns>
        public static ResultMo<OauthAccessTokenMo> ConvertToComMo(this WxGetOauthAccessTokenResp wxMo)
        {
            if (!wxMo.IsSuccess())
                return wxMo.ConvertToResultOnly<OauthAccessTokenMo>();

            var nowTimestamp = DateTime.Now.ToUtcSeconds();
            var appInfo = MemberShiper.AppAuthorize;

            var comMo = new OauthAccessTokenMo
            {
                access_token = wxMo.access_token,
                expire_date = nowTimestamp + wxMo.expires_in,
                refresh_token = wxMo.refresh_token,
                create_time = nowTimestamp,

                app_user_id = wxMo.openid,
                tenant_id = appInfo.TenantId.ToInt64()
            };
            return new ResultMo<OauthAccessTokenMo>(comMo);
        }


        /// <summary>
        /// 转化到授权实体
        /// </summary>
        /// <param name="wxMo"></param>
        /// <returns></returns>
        public static ResultMo<OauthUserMo> ConvertToComMo(this WxGetOauthUserResp wxMo)
        {
            if (!wxMo.IsSuccess())
                return wxMo.ConvertToResultOnly<OauthUserMo>();

            var appInfo = MemberShiper.AppAuthorize;
            var comMo = new OauthUserMo
            {
                app_user_id = wxMo.openid,
                app_union_id = wxMo.unionid,
                sex = (Sex)wxMo.sex,
                nick_name = wxMo.nickname,
                platform = SocialPaltforms.Wechat,

                head_img = wxMo.headimgurl,
                create_time = DateTime.Now.ToUtcSeconds(),
                tenant_id = appInfo.TenantId.ToInt64()
            };
            return new ResultMo<OauthUserMo>(comMo);
        }
        
    }
}
