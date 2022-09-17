
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

namespace OSS.Core.Module.Article;

/// <summary>
///  专题 对象实体 
/// </summary>
public class TopicMo : BaseOwnerAndStateMo<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///   专题logo
    /// </summary>
    public string? avatar { get; set; }

    /// <summary>
    ///  专题简介
    /// </summary>
    public string brief { get; set; }
}
