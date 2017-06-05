namespace OSS.Core.Infrastructure.Utils
{
    public class GlobalKeysUtil
    {
        #region  cookie等名称

        /// <summary>
        ///  验证票据名称
        /// </summary>
        public const string AuthorizeTicketName = "at_id";

        /// <summary>
        ///  用户验证cookie名称
        /// </summary>
        public const string UserCookieName = "ct_id";

        /// <summary>
        /// 管理员验证cookie名称
        /// </summary>
        public const string AdminCookieName = "cat_id";


        /// <summary>
        ///  用户登录注册成功后跳转来源地址的cookie名称
        /// </summary>
        public const string UserReturnUrlCookieName = "r_url";

        #endregion


    }
}
