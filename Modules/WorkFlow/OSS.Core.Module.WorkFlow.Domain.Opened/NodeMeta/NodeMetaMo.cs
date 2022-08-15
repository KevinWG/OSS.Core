
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

namespace OSS.Core.Module.WorkFlow;

/// <summary>
///  节点配置实体 
/// </summary>
public class NodeMetaMo : BaseOwnerAndStateMo<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    ///  编号
    /// </summary>
    public string code { get; set; }

    /// <summary>
    ///  节点类型
    /// </summary>
    public NodeType node_type { get; set; }

    /// <summary>
    /// 节点实例类型
    /// </summary>
    public NodeInsType ins_type { get; set; }

}