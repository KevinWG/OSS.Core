
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
using OSS.Common.ComModels;
using OSS.Common.Extention;
using OSS.Core.Domains.Sns.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.SnsSdk.Oauth.Wx.Mos;

namespace OSS.Core.Services.Sns.Oauth.Handlers.Extention
{
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

            var comMo = new OauthAccessTokenMo
            {
                access_token = wxMo.access_token,
                expire_date = nowTimestamp + wxMo.expires_in,
                refresh_token = wxMo.refresh_token,
                create_time = nowTimestamp,

                app_user_id = wxMo.openid
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

            var comMo = new OauthUserMo
            {
                app_user_id = wxMo.openid,
                app_union_id = wxMo.unionid,
                sex = (Sex)wxMo.sex,
                nick_name = wxMo.nickname,
                platform = ThirdPaltforms.Wechat,

                head_img = wxMo.headimgurl
            };
            return new ResultMo<OauthUserMo>(comMo);
        }

        /// <summary>
        ///  设置token相关的信息
        /// </summary>
        /// <param name="userMo"></param>
        /// <param name="accessMo"></param>
        public static void SetTokenInfo(this OauthUserMo userMo, OauthAccessTokenMo accessMo)
        { 
            userMo.access_token = accessMo.access_token;
            userMo.expire_date = accessMo.expire_date;
            userMo.refresh_token = accessMo.refresh_token;
            userMo.create_time = accessMo.create_time;
        }

    }
}
