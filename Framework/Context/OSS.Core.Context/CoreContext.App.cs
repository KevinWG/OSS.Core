using OSS.Common;
using OSS.Common.Resp;

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
            ///  应用信息是否初始化完成
            /// </summary>
            public static bool IsInitialized = ContextHelper.GetContext().AppIdentity != null;


            /// <summary>
            ///  客户端应用授权认证信息
            /// </summary>
            public static AppIdentity Identity
            {
                get
                {
                    var identity = ContextHelper.GetContext().AppIdentity;
                    if (identity == null)
                    {
                        throw new RespException(SysRespCodes.NotImplement, $"未能获取有效当前应用信息!");
                    }

                    return identity;
                }

                set => ContextHelper.SetAppIdentity(value);
            }


            private static readonly AppInfo _defaultAppInfo = new() {AppWorkerId = 1, AppVersion = "1.0"};

            /// <summary>
            ///  自身应用信息
            /// </summary>
            public static AppInfo Self { get; set; } = _defaultAppInfo;
        }
    }

    /// <summary>
    ///  应用信息
    /// </summary>
    public class AppInfo//:IAccessSecret
    {
        /// <summary>
        ///  当前应用工作实例Id
        /// </summary>
        public  int AppWorkerId { get; set; }   

        /// <summary>
        /// 当前应用版本
        /// </summary>
        public  string AppVersion { get; set; } = string.Empty;


        /// <summary>
        ///  应用访问秘钥-key
        /// </summary>
        public string AccessKey { get; set; } = string.Empty;

        /// <summary>
        ///  应用访问秘钥-secret
        /// </summary>
        public string AccessSecret { get; set; } = string.Empty;

    }

}
