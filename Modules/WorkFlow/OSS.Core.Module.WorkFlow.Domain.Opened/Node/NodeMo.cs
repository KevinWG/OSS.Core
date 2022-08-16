
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
///  Node 对象实体 
/// </summary>
public class NodeMo : BaseOwnerAndStateMo<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; } 
    
    /// <summary>
    /// 节点实例类型
    /// </summary>
    public NodeInsType ins_type { get; set; }
}

public enum NodeInsType
{
    /// <summary>
    /// 开始节点
    /// </summary>
    StartNode = 0,

    /// <summary>
    /// 结束节点
    /// </summary>
    EndNode = 1000000,



    /// <summary>
    /// 自定义
    /// </summary>
    Custom = 100000,




    /// <summary>
    ///  管理员审核节点
    /// </summary>
    AdminAudit = 100,

    /// <summary>
    ///  用户确认节点
    /// </summary>
    UserConfirm = 110,
}
