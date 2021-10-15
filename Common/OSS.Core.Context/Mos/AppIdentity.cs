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
        ///  请求模块名称
        /// </summary>
        public string module_name { get; set; }

        /// <summary>
        ///  当前请求主机信息 [非外部传值，不参与签名]
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 来源模式
        /// </summary> 
        public AppSourceMode source_mode { get; set; }

        /// <summary>
        ///  请求验证对应功能权限信息
        /// </summary>
        public AskUserFunc ask_func { get; set; }
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
        OutApp = 100,


        /// <summary>
        /// 浏览器访问模式
        /// </summary>
        Browser = 300
    }
}