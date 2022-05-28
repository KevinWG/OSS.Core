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
            public static bool IsAuthenticated => Identity != null;

            #region  成员信息

            /// <summary>
            ///   用户认证信息
            /// </summary>
            public static UserIdentity Identity
            {
                get { return ContextHelper.GetContext()?.MemberIdentity; }
                set { ContextHelper.SetMemberIdentity(value); }

            }
            #endregion
        }

    }
}
