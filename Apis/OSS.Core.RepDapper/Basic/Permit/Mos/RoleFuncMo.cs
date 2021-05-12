using OSS.Core.Infrastructure.BasicMos.Enums;
using OSS.Core.Infrastructure.BasicMos;

namespace OSS.Core.RepDapper.Basic.Permit.Mos
{
    public class RoleFuncMo : BaseOwnerAndStateMo
    {
        /// <summary>
        ///  角色Id
        /// </summary>
        public string role_id { get; set; }

        /// <summary>
        /// 权限码
        /// </summary>
        public string func_code { get; set; }

        /// <summary>
        ///  数据权限
        /// </summary>
        public FuncDataLevel data_level { get; set; }
    }

    public class RoleFunSmallMo
    {
        /// <summary>
        /// 权限码
        /// </summary>
        public string func_code { get; set; }

        /// <summary> 
        ///  数据权限
        /// </summary>
        public FuncDataLevel data_level { get; set; }
    }
}
