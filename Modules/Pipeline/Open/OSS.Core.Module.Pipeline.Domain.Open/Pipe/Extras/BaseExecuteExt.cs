namespace OSS.Core.Module.Pipeline;

/// <summary>
///  管道扩展信息基类
/// </summary>
public class BaseExecuteExt
{
}

public class DefaultExecuteExt: BaseExecuteExt
{
    public static DefaultExecuteExt Default = new DefaultExecuteExt();
}