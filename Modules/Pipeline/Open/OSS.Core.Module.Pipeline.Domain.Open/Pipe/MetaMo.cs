
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
///  流水线元数据定义实体
/// </summary>
public class MetaMo : BaseOwnerMo<long>
{
    /// <summary>
     ///  最近使用流水线管道Id
     /// </summary>
     public long latest_pipe_id { get; set; }
}