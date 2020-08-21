using System.Xml.Serialization;

namespace OSS.CorePro.TAdminSite.Apis.Permit.Reqs
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

   
}
