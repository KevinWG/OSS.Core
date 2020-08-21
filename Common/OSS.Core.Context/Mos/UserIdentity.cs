namespace OSS.Core.Context.Mos
{
    /// <summary>
    /// 成员通行证信息
    /// </summary>
    public class UserIdentity
    {
        /// <summary>
        ///    授权账号来源
        /// </summary>
        public int from_plat { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public PortalAuthorizeType auth_type { get; set; }

        /// <summary>
        ///  授权编号
        /// </summary>
        public string id { get; set; }
        
        /// <summary>
        ///  展示名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///  展示图片
        /// </summary>
        public string avatar { get; set; }
    }
    
    /// <summary>
    ///     成员授权类型
    /// </summary>
    public enum PortalAuthorizeType
    {
        /// <summary>
        ///     超级管理员
        /// </summary>
        SuperAdmin = 100,

        /// <summary>
        ///     后台普通管理员
        /// </summary>
        Admin = 200,

        /// <summary>
        ///    用户
        /// </summary>
        User = 300,

        /// <summary>
        ///     第三方临时授权用户 (页面过渡第三方和系统用户绑定时使用
        /// </summary>
        OauthTemp = 400
    }
}