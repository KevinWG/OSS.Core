namespace OSS.Core.Context
{
    /// <summary>
    /// OSSCore 核心上下文信息
    /// </summary>
    public static partial class CoreContext
    {
        /// <summary>
        ///  上下文应用信息
        /// </summary>
        public static class App
        {
            /// <summary>
            ///  客户端应用授权认证信息
            /// </summary>
            public static AppIdentity Identity
            {
                get => ContextHelper.GetContext()?.AppIdentity;
                set => ContextHelper.SetAppIdentity(value);
            }

            /// <summary>
            ///  自身应用信息
            /// </summary>
            public static AppInfo Self {
                get; set;
            }
        }

    }


    public class AppInfo
    {
        /// <summary>
        ///  当前应用Id
        /// </summary>
        public  string AppId { get; set; } //= ConfigHelper.GetSection("AppConfig:AppId")?.Value;

        /// <summary>
        ///  当前应用秘钥
        /// </summary>
        public  string AppSecret { get; set; }  /*ConfigHelper.GetSection("AppConfig:AppSecret")?.Value;*/

        /// <summary>
        ///  当前应用工作实例Id
        /// </summary>
        public  int AppWorkerId { get; set; }    /*ConfigHelper.GetSection("AppConfig:AppWorkerId")?.Value.ToInt32() ?? 0;*/

        /// <summary>
        /// 当前应用版本
        /// </summary>
        public  string AppVersion { get; set; }  /* ConfigHelper.GetSection("AppConfig:AppVersion")?.Value ?? string.Empty;*/
    }
}
