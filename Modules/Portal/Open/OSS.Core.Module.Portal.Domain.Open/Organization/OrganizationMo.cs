
#region Copyright (C)  Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 实体对象
*
*　　	创建人： osscore
*    	创建日期：
*       
*****************************************************************************/

#endregion

using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

/// <summary>
///  组织机构 对象实体 
/// </summary>
public class OrganizationMo : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    /// 组织机构名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///  组织机构名称
    /// </summary>
    public OrgType org_type { get; set; }
}


public enum OrgType
{
    /// <summary>
    ///  客户
    /// </summary>
    Customer = 0,

    /// <summary>
    ///  供应商
    /// </summary>
    supplier = 100,

    /// <summary>
    /// 加工制造商
    /// </summary>
    Manufacturer =200,

    /// <summary>
    ///  其他
    /// </summary>
    Other = 1000,
}
