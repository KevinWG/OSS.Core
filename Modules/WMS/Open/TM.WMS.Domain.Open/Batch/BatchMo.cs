
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

namespace TM.WMS;

/// <summary>
///  批次号 对象实体 
/// </summary>
public class BatchMo : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    /// 编码
    /// </summary>
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///  物料Id
    /// </summary>
    public long material_id { get; set; }

    /// <summary>
    ///  物料编码
    /// </summary>
    public string material_code { get; set; } = string.Empty;

    /// <summary>
    ///  过期时间
    /// </summary>
    public long expire_date { get; set; }

    /// <summary>
    ///  备注信息
    /// </summary>
    public string remark { get; set; } = string.Empty;
}
