using OSS.Core.Infrastructure.BasicMos.Enums;

namespace OSS.CorePro.TAdminSite.Apis.Permit.Reqs
{
    public class GetRoleItemResp
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

    public static class GetRoleItemRespMap
    {
        public static GetRoleItemResp ToRoleItemResp(this FuncItem funcItem)
        {
            return new GetRoleItemResp()
            {
                func_code  = funcItem.code,
                data_level = FuncDataLevel.All
            };
        }
    }



}
