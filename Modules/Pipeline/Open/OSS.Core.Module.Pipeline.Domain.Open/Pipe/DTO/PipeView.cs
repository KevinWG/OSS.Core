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
///  管道对象实体 
/// </summary>
public class PipeView: BasePieView
{
    /// <summary>
    ///  管道状态
    /// </summary>
    public CommonStatus status { get; set; }


    /// <summary>
    /// 父级 Pipeline id
    /// </summary>
    public long parent_id { get; set; }

}



public class BasePieView 
{
    /// <summary>
    ///  管道id
    /// </summary>
    public long id { get; set; }

    /// <summary>
    /// 管道节点名称
    /// </summary>
    public string name { get; set; } = default!;

    /// <summary>
    /// 管道类型
    /// </summary>
    public PipeType type { get; set; }

    /// <summary>
    ///  执行扩展信息
    /// </summary>
    public BaseExecuteExt execute_ext { get; set; } = default!;

    /// <summary>
    ///  添加时间
    /// </summary>
    public long add_time { get; set; }
}