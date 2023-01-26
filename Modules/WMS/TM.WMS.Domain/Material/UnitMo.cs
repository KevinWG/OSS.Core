using OSS.Core.Domain;

namespace TM.WMS
{
    public class UnitMo:BaseTenantOwnerAndStateMo<long>
    {
        /// <summary>
        ///  名称
        /// </summary>
        public string name { get; set; } = string.Empty;
    }


    public static class UnitMoMap
    {
        /// <summary>
        ///  转化视图实体
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        public static UnitView ToView(this UnitMo mo)
        {
            return new UnitView()
            {
                id   = mo.id,
                name = mo.name,
            };
        }


        /// <summary>
        ///  转化为单位对象实体
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static UnitMo ToMo(this AddUnitReq req)
        {
            var mo = new UnitMo
            {
                name = req.name
            };

            mo.FormatBaseByContext();
            return mo;
        }
    }
}
