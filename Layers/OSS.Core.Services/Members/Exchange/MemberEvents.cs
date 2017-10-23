using OSS.Common.Authrization;
using OSS.Core.Domains.Members.Mos;

namespace OSS.Core.Services.Members.Exchange
{
    internal static class MemberEvents
    {
        #region 触发方法
        
        public static void TriggerUserRegiteEvent(UserInfoBigMo arg1, AppAuthorizeInfo arg2)
        {
            // todo  推送注册消息
        }

        public static void TriggerUserLoginEvent(UserInfoBigMo arg1, AppAuthorizeInfo arg2)
        {
            // todo  推送登录消息
        }
        #endregion
    }
}