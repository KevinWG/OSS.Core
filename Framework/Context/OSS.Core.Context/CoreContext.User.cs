using OSS.Common.Resp;

namespace OSS.Core.Context
{
    public static partial class CoreContext
    {
        /// <summary>
        /// 用户上下文信息
        /// </summary>
        public static class User
        {
            /// <summary>
            /// 是否已经授权认证
            /// </summary>
            public static bool IsAuthenticated => ContextHelper.GetContext().MemberIdentity != null;


            #region 成员信息

            /// <summary>
            ///   用户认证信息
            /// </summary>
            public static UserIdentity Identity
            {
                get
                {
                    var identity = ContextHelper.GetContext().MemberIdentity;
                    if (identity == null)
                    {
                        throw new RespException(RespCodes.UserUnLogin, "当前用户未登录!");
                    }

                    return identity;
                }

                set => ContextHelper.SetMemberIdentity(value);
            }

            #endregion
        }

    }
}
