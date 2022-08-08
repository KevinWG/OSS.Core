

using OSS.Core.Context;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

public class RoleFuncMo : BaseOwnerAndStateMo<long>
{
    /// <summary>
    ///  角色Id
    /// </summary>
    public long role_id { get; set; }

    /// <summary>
    /// 权限码
    /// </summary>
    public string func_code { get; set; } = string.Empty;

    /// <summary>
    ///  数据权限
    /// </summary>
    public FuncDataLevel data_level { get; set; }
}
