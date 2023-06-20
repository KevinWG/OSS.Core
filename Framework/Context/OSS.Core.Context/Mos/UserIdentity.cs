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
        [Obsolete("使用 id_type，此属性 3.0 版本后禁用")]
        public AuthorizeType auth_type => (AuthorizeType)type;
        
        /// <summary>
        /// 授权身份类
        /// </summary>
        public UserIdentityType type { get; set; }

        /// <summary>
        ///  身份id
        /// </summary>
        public string id { get; set; } = string.Empty;

        /// <summary>
        ///  身份名称
        /// </summary>
        public string name { get; set; } = string.Empty;

        /// <summary>
        ///  展示图片
        /// </summary>
        public string? avatar { get; set; }

        /// <summary>
        ///  数据权限层级
        /// </summary>
        public FuncDataLevel data_level { get; set; } = FuncDataLevel.OnlySelf;

        /// <summary>
        ///  用户扩展信息
        /// </summary>
        public object? ext { get; set; }
    }

    /// <summary>
    ///     成员授权类型
    /// </summary>
    [Obsolete]
    public enum AuthorizeType
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
    ///    授权身份类型
    /// </summary>
    public enum UserIdentityType
    {
        /// <summary>
        ///  超级管理员 - 【管理员】
        /// </summary>
        SuperAdmin = 100,

        /// <summary>
        ///  后台管理员 - 【管理员/员工】
        /// </summary>
        Admin = 200,

        /// <summary>
        ///   普通用户
        /// </summary>
        NormalUser = 300,

        /// <summary>
        ///   空白普通用户
        /// （关键信息缺失，如手机号）
        /// </summary>
        NormalUserWithEmpty = 310,

        /// <summary>
        ///  第三方授权的【临时用户】
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
        All = 100,

        /// <summary>
        ///  组织树
        /// </summary>
        GroupTree = 200,

        /// <summary>
        /// 当前组织
        /// </summary>
        Group = 300,

        /// <summary>
        ///  仅个人
        /// </summary>
        OnlySelf = 1000
    }
}