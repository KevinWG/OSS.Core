using System.Collections.Generic;
using System.Xml.Serialization;

namespace OSS.Core.Services.Plugs.Log.Mos
{
    public class LogReceiverConfig
    {
        /// <summary>
        ///  默认接收人
        /// </summary>
        public string default_receivers { get; set; }

        /// <summary>
        ///   接收设置列表
        /// </summary>
        [XmlArray("items")]
        [XmlArrayItem("item")]
        public List<LogConfigItem> items { get; set; }
    }

    public class LogConfigItem
    {
        /// <summary>
        ///  模块名称
        /// </summary>
        [XmlAttribute]
        public string module_name { get; set; }

        /// <summary>
        ///  消息key
        /// </summary>
        [XmlAttribute]
        public string msg_key { get; set; }


        /// <summary>
        ///  消息接收人
        /// </summary>
        [XmlAttribute]
        public string receivers { get; set; }
    }
}
