using OSS.Core.Infrastructure.BasicMos.Enums;
using System.Xml.Serialization;
using OSS.Core.RepDapper.Basic.Permit.Mos;

namespace OSS.Core.AdminSite.Apis.Permit.Reqs
{

    public class FuncBigItem : FuncItem
    {
        public string parent_code { get; set; }
    }

    public class FuncItem
    {
        /// <summary>
        ///  权限标题名称
        /// </summary>
        [XmlAttribute]
        public string title { get; set; }

        /// <summary>
        /// 权限码
        /// </summary>
        [XmlAttribute]
        public string code { get; set; }
    }



    public static class FuncBigItemMap
    {
        public static RoleFunSmallMo ToSmallMo(this FuncBigItem item)
        {
            var mo = new RoleFunSmallMo();
            mo.func_code = item.code;
            mo.data_level = FuncDataLevel.All;
            return mo;
        }
    }


}
