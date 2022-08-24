namespace OSS.Core.Module.Pipeline;

public static class PipeMoExtension
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
            name      = req.name,
            type      = req.type,
            parent_id = req.parent_id,
        };
        return mo;
    }

    /// <summary>
    /// 转化为Dto对象
    /// </summary>
    /// <param name="pipe"></param>
    /// <returns></returns>
    public static PipeView ToView(this PipeMo pipe)
    {
        var view = new PipeView
        {
            id = pipe.id,

            name   = pipe.name,
            status = pipe.status,

            add_time = pipe.add_time,
            parent_id = pipe.parent_id
        };

        view.FormatByIPipe(pipe);

        return view;
    }

    /// <summary>
    ///  通过基础管道仓储实体属性格式化
    /// </summary>
    /// <param name="view"></param>
    /// <param name="pipe"></param>
    public static void FormatByIPipe(this BasePieView view, IPipeProperty pipe)
    {
        view.type        = pipe.type;
        view.execute_ext = GetExecuteExtra(pipe.type, pipe.execute_ext);
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