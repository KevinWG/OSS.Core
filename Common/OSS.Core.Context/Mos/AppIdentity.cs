#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：通用系统授权信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

namespace OSS.Core.Context
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
        ///  请求模块名称
        /// </summary>
        public string module_name { get; set; }

        /// <summary>
        /// 来源模式
        /// </summary> 
        public AppSourceMode SourceMode { get; set; }

        /// <summary>
        ///  请求验证对应功能权限信息
        /// </summary>
        public AskUserFunc ask_func { get; set; } 

        /// <summary>
        /// 复制新的授权信息实体
        /// </summary>
        /// <returns></returns>
        public AppAuthInfo Copy()
        {
            var newOne = new AppIdentity
            {
                ask_func = this.ask_func,
                app_client = this.app_client,
                app_id = this.app_id,
                app_ver = this.app_ver,
                client_ip = this.client_ip,

                sign = this.sign,
                tenant_id = this.tenant_id,
                timestamp = this.timestamp,
                token = this.token,
                trace_no = this.trace_no,

                UDID = this.UDID,
                app_type = this.app_type
            };

            return newOne;
        }
        
        /// <summary>
        ///  当前请求主机信息 [非外部传值，不参与签名]
        /// </summary>
        public string host { get; set; }
    }

    /// <summary>
    ///  要求的功能权限信息
    /// </summary>
    public class AskUserFunc
    {
        /// <summary>
        /// 功能权限要求
        /// </summary>
        /// <param name="authType"></param>
        /// <param name="funcCode"></param>
        /// <param name="sceneCode"></param>
        public AskUserFunc(PortalAuthorizeType authType, string funcCode, string sceneCode)
        {
            func_code = funcCode;
            scene_code     = sceneCode;
            auth_type = authType;
        }

        /// <summary>
        ///  权限码
        /// </summary>
        public string func_code { get; }

        /// <summary>
        ///  业务场景
        /// </summary>
        public string scene_code { get; }

        /// <summary>
        ///  要求的登录类型限制
        /// </summary>
        public PortalAuthorizeType auth_type { get; } 
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
        ///  服务器端（如：站点，WindowsService）
        /// </summary>
        Server = 60
    }

 
    /// <summary>
    ///  app的来源处理模式
    /// </summary>
    public enum AppSourceMode
    { 
        /// <summary>
        ///  应用签名模式（强签名）
        /// </summary>
        AppSign = 0,


        /// <summary>
        ///  合作方应用模式（自定义约定验证模式
        /// </summary>
        PartnerApp = 100,


        /// <summary>
        /// 浏览器访问模式
        /// </summary>
        Browser = 300,

    }
}