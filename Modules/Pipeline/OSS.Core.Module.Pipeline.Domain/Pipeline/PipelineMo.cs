using System.Text.Json;
using System.Text.Json.Serialization;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线信息（含通用管道信息部分）
/// </summary>
public class PipelineMo:VersionMo,IPipeProperty
{
    /// <summary>
    /// 管道类型
    /// </summary>
    public PipeType type { get; set; }

    /// <summary>
    ///  扩展信息
    /// </summary>
    public string? execute_ext { get; set; }

    /// <summary>
    ///  父级 Pipeline id
    /// </summary>
    public long parent_id { get; set; }
}



public static class PipelineMoExtension
{
    /// <summary>
    ///  转化为View对象
    /// </summary>
    /// <param name="pipeline"></param>
    /// <returns></returns>
    public static PipelineView ToView(this PipelineMo pipeline)
    {
        var view = new PipelineView();

        view.id = pipeline.id;
        view.add_time = pipeline.add_time;
        view.status = pipeline.status;

        view.name     = pipeline.name;
        view.meta_id  = pipeline.meta_id;
        view.ver_name = pipeline.ver_name;
        
        var links = string.IsNullOrEmpty(pipeline.links)
            ? new List<Link>() 
            : JsonSerializer.Deserialize<List<Link>>(pipeline.links);

        view.links = links??new List<Link>();

        view.FormatByIPipe(pipeline);

        return view;
    }



}