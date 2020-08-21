using OSS.Core.Infrastructure.BasicMos.Enums;

namespace OSS.Core.Services.Basic.Permit.Mos
{
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
