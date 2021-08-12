using OSS.Core.Context;
using OSS.Core.RepDapper.Basic.SocialPlats.Mos;

namespace OSS.Core.Services.Basic.Portal.Events
{
    internal static class PortalEvents
    {
        #region 触发方法

        public static void TriggerRegisterEvent(UserIdentity identity, SocialPlatform plat, AppIdentity app)
        {

        }

        public static void TriggerOauthTempLoginEvent(UserIdentity identity, AppIdentity app, SocialPlatform plat = SocialPlatform.None)
        {

        }

        public static void TriggerLoginEvent(UserIdentity identity, AppIdentity app, SocialPlatform plat=SocialPlatform.None)
        {
            // todo  异步推送登录消息
            //return Task.CompletedTask;
            // var authType = (PortalAuthorizeType)identity.AuthenticationType;
        }
        
        #endregion
    }
}