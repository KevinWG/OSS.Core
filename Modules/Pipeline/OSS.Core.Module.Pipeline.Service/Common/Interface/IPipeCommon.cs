namespace OSS.Core.Module.Pipeline;

internal interface IPipeCommon
{
    /// <summary>
    ///  添加管道
    /// </summary>
    /// <param name="newPipe"></param>
    /// <returns></returns>
    internal Task AddPipe(PipeMo newPipe);
}