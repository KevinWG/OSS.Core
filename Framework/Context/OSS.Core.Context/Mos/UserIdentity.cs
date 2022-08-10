namespace OSS.Core.Context
{
    /// <summary>
    /// 成员通行证信息
    /// </summary>
    public class UserIdentity
    {
        /// <summary>
        /// 授权类型
        /// </summary>
        public PortalAuthorizeType auth_type { get; set; }

        /// <summary>
        ///  授权编号
        /// </summary>
        public string id { get; set; } = string.Empty;

        /// <summary>
        ///  展示名称
        /// </summary>
        public string name { get; set; } = string.Empty;

        /// <summary>
        ///  展示图片
        /// </summary>
        public string? avatar { get; set; }

        /// <summary>
        ///  数据权限层级
        /// </summary>
        public FuncDataLevel data_level { get; set; } = FuncDataLevel.All;

        /// <summary>
        ///  用户扩展信息
        /// </summary>
        public object? ext { get; set; }
    }

    /// <summary>
    ///     成员授权类型
    /// </summary>
    public enum PortalAuthorizeType
    {
        /// <summary>
        ///     超级管理员 - 管理端
        /// </summary>
        SuperAdmin = 100,

        /// <summary>
        ///     后台普通管理员- 管理端
        /// </summary>
        Admin = 200,

        /// <summary>
        ///    用户
        /// </summary>
        User = 300,

        /// <summary>
        ///   空白用户（关键信息缺失，如手机号）
        /// </summary>
        UserWithEmpty = 310,

        /// <summary>
        ///  临时授权的第三方用户
        /// </summary>
        SocialAppUser = 400
    }
    
    /// <summary>
    /// 功能数据权限
    /// </summary>
    public enum FuncDataLevel
    {
        /// <summary>
        ///  全部
        /// </summary>
        All = 1,

        /// <summary>
        ///  组织树
        /// </summary>
        GroupTree = 20,

        /// <summary>
        /// 当前组织
        /// </summary>
        Group = 40,

        /// <summary>
        ///  仅个人
        /// </summary>
        OnlySelf = 60
    }
}