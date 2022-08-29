
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

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线信息（专用字段部分）
/// </summary>
public class PipelinePartMo :BaseOwnerMo<long>, IDomainStatus<PipelineStatus> //, IPipeProperty
{
    /// <summary>
    ///  定义Id
    /// </summary>
    public long meta_id { get; set; }

    /// <summary>
    /// 流水线名称
    /// </summary>
    public string name { get; set; } = default!;

    /// <summary>
    ///   版本名称
    /// </summary>
    public string ver_name { get; set; } = string.Empty;
    
    /// <summary>
    ///  当前版本的链接信息
    /// </summary>
    public string? links { get; set; }

    /// <summary>
    /// 版本状态
    /// </summary>
    public PipelineStatus status { get; set; }
}
