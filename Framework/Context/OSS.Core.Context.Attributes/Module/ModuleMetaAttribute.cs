using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes;

/// <summary>
///  模块配置信息属性
/// </summary>
public class ModuleMetaAttribute : BaseOrderAuthorizeAttribute
{
    private readonly string _moduleName;


    /// <summary>
    ///  模块配置信息属性
    /// </summary>
    /// <param name="moduleName"></param>
    public ModuleMetaAttribute(string moduleName)
    {
        Order = AttributeConst.Order_Module_MetaAttribute;
        _moduleName = moduleName;
    }

    /// <summary>
    ///  校验模块信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Task<IResp> Authorize(AuthorizationFilterContext context)
    {
        if (string.IsNullOrEmpty(_moduleName))
            throw new NullReferenceException("请在当前Controller或父类中使用ModuleNameAttribute标注模块名称！");

        if (!CoreContext.App.IsInitialized)
        {
            CoreContext.App.Identity = new AppIdentity();
        }

        var appInfo = CoreContext.App.Identity;

        appInfo.module_name = _moduleName;

        return Task.FromResult((IResp)Resp.Success());
    }
}