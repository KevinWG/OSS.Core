using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 领域对象开放接口
/// </summary>
public interface IPipeOpenService
{

    /// <summary>
    ///  添加管道
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<IResp<long>> Add(AddPipeReq req);
}
