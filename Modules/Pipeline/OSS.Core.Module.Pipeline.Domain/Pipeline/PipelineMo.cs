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
        var view = new PipelineView();

        view.id = mo.id;
        view.add_time = mo.add_time;
        view.status = mo.status;

        view.name     = mo.name;
        view.meta_id  = mo.meta_id;
        view.ver_name = mo.ver_name;
        

        view.FormatByIPipe(mo);

        return view;
    }



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
}