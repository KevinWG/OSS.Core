
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

namespace TM.Module.Product;

/// <summary>
///  产品 对象实体 
/// </summary>
public class SpuMo : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string title { get; set; } = string.Empty;

    /// <summary>
    ///  描述
    /// </summary>
    public string? desp { get; set; } 


    
}
