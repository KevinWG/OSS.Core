#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：通用系统授权信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

namespace OSS.Core.Context.Mos
{
    /// <summary>
    ///   应用的授权认证信息
    /// </summary>
    public class AppIdentity : AppAuthInfo
    {
        /// <summary>
        /// 应用客户端类型[非外部传值，不参与签名]
        /// </summary>
        public AppClientType app_client { get; set; }

        /// <summary>
        ///   应用类型 [非外部传值，不参与签名]
        /// </summary>
        public AppType app_type { get; set; } = AppType.Outer;

        /// <summary>
        ///  请求模块名称
        /// </summary>
        public string module_name { get; set; }

        /// <summary>
        /// 是否是合作调用
        /// </summary> 
        public bool is_partner { get; set; }

        /// <summary>
        ///  请求验证对应权限码
        /// </summary>
        public string func { get; set; }

        /// <summary>
        /// 复制新的授权信息实体
        /// </summary>
        /// <returns></returns>
        public AppAuthInfo Copy()
        {
            var newOne = new AppIdentity
            {
                func      = this.func,
                app_client = this.app_client,
                app_id    = this.app_id,
                app_ver    = this.app_ver,
                client_ip = this.client_ip,

                sign      = this.sign,
                tenant_id = this.tenant_id,
                timestamp = this.timestamp,
                token     = this.token,
                trace_num = this.trace_num,

                UDID    = this.UDID,
                app_type = this.app_type
            };

            return newOne;
        }

    }

    /// <summary>
    /// 应用客户端类型
    /// </summary>
    public enum AppClientType
    {
        /// <summary>
        ///  未知
        /// </summary>
        UnknowClient = 10,

        /// <summary>
        /// 苹果客户端应用
        /// </summary>
        iOS = 20,

        /// <summary>
        /// 安卓客户端应用
        /// </summary>
        Android = 30,

        /// <summary>
        ///  windows客户端应用
        /// </summary>
        WindowStore = 40,

        /// <summary>
        ///  服务器端（如：Web，WindowsService）
        /// </summary>
        Server = 60
    }

    /// <summary>
    ///  应用类型
    /// </summary>
    public enum AppType
    {
        /// <summary>
        ///  平台管理应用
        /// </summary>
        SystemManager = 1,

        /// <summary>
        ///  平台应用
        /// </summary>
        System = 30,

        /// <summary>
        ///  多租户代理应用 
        /// </summary>
        Proxy = 60,

        /// <summary>
        ///  内部单租户应用
        /// </summary>
        Inner = 90,

        /// <summary>
        ///   外部单租户应用
        /// </summary>
        Outer = 120
    }
}