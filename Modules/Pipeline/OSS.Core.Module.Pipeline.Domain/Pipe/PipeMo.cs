using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  管道对象实体 
/// </summary>
public class PipeMo : BaseOwnerAndStateMo<long>
{
    /// <summary>
    /// 管道节点名称
    /// </summary>
    public string name { get; set; } = default!;

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

    /// <summary>
    ///  管道状态
    /// </summary>
    public CommonStatus status { get; set; }
}


public static class PipeMoMap
{
    /// <summary>
    ///  转化为管道对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static PipeMo MapToPipeMo(this AddPipeReq req)
    {
        var mo = new PipeMo
        {
            name = req.name,
            type = req.type,
            parent_id = req.parent_id,
        };
        return mo;
    }


    public static PipeItem ToDetail(this PipeMo pipe)
    {
        var detail = new PipeItem
        {
            id   = pipe.id,
            name = pipe.name,
            type = pipe.type,

            status      = pipe.status,
            parent_id   = pipe.parent_id,
            execute_ext = GetExecuteExtra(pipe.type, pipe.execute_ext)
        };
        return detail;
    }

    private static BaseExecuteExt GetExecuteExtra(PipeType type, string? executeExt)
    {
        if (string.IsNullOrEmpty(executeExt))
        {
            return DefaultExecuteExt.Default;
        }

        switch (type)
        {
            // todo  完善配置处理
        }

        return DefaultExecuteExt.Default;

    }
}
