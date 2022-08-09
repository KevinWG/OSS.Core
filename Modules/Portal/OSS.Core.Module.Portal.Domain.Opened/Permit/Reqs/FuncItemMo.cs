
using OSS.Core.Context;

namespace OSS.Core.Module.Portal
{
    //public class FuncBigItem : FuncItem
    //{
    //    public string parent_code { get; set; }
    //}

    //public class FuncItem
    //{
    //    /// <summary>
    //    ///  权限标题名称
    //    /// </summary>
    //    [XmlAttribute]
    //    public string title { get; set; }

    //    /// <summary>
    //    /// 权限码
    //    /// </summary>
    //    [XmlAttribute]
    //    public string code { get; set; }
    //}

    public static class FuncBigItemMap
    {
        public static GrantedPermit ToSmallMo(this FuncMo item)
        {
            var mo = new GrantedPermit
            {
                func_code = item.code,
                data_level = FuncDataLevel.All
            };
            return mo;
        }
    }
}
