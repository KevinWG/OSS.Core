using OSS.Core.Context;

namespace OSS.Core.Module.Portal;

public class GrantedPermit
{
    /// <summary>
    /// 权限码
    /// </summary>
    public string func_code { get; set; } = string.Empty;

    /// <summary> 
    ///  数据权限
    /// </summary>
    public FuncDataLevel data_level { get; set; }
}