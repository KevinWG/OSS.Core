using System.Text.Json;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线信息（含通用管道信息部分）
/// </summary>
public class PipelineMo:PipelinePartMo,IPipeProperty
{
    /// <summary>
    /// 管道类型
    /// </summary>
    public PipeType type { get; set; }

    /// <summary>
    ///  扩展信息
    /// </summary>
    public string? execute_ext { get; set; }
}



public static class PipelineMoExtension
{
    /// <summary>
    ///  转化为View对象
    /// </summary>
    /// <param name="mo"></param>
    /// <returns></returns>
    public static PipelineView ToView(this PipelineMo mo)
    {
        var view = new PipelineView
        {
            id       = mo.id,
            status   = mo.status,
            name     = mo.name,

            meta_id  = mo.meta_id,
            ver_name = mo.ver_name,
            add_time = mo.add_time
        };

        view.FormatByIPipe(mo);

        return view;
    }


    /// <summary>
    ///  转化为详情视图实体
    /// </summary>
    /// <param name="mo"></param>
    /// <returns></returns>
    public static PipelineDetailView ToDetailView(this PipelineMo mo)
    {
        var pipeline = mo.ToView();

        var links =(string.IsNullOrEmpty(mo.links)
            ? null
            : JsonSerializer.Deserialize<List<Link>>(mo.links)) ?? new List<Link>();
        
        return new PipelineDetailView()
        {
            pipeline = pipeline,
            links    = links
        };
    }

    /// <summary>
    /// 转化为业务流节点实体
    /// </summary>
    /// <param name="pipeline"></param>
    /// <returns></returns>
    public static FlowNodeMo ToFlowMo(this PipelineMo pipeline)
    {
        var mo = new FlowNodeMo
        {
            pipe_id   = pipeline.id,
            pipe_type = pipeline.type,
            title     = pipeline.name
        };
        return mo;
    }
}