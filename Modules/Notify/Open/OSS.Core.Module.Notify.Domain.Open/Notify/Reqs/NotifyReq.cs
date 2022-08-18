using OSS.Common.Resp;

namespace OSS.Core.Module.Notify
{
    public class NotifySendReq
    {        /// <summary>
        ///  通知请求
        /// </summary>
        public NotifySendReq()
        {

        }

        /// <summary>
        /// 目标账号
        /// </summary>
        public IList<string> targets { get; set; }

        /// <summary>
        ///  模板编号
        /// </summary>
        public long template_id { get; set; }

        /// <summary>
        ///  消息Id
        /// </summary>
        public string msg_Id { get; set; } = string.Empty;

        /// <summary>
        ///  消息标题
        ///    如若为空，取模板的标题
        /// </summary>
        public string msg_title { get; set; } = string.Empty;

        /// <summary>
        ///  内容数据
        /// </summary>
        public Dictionary<string, string>? body_paras { get; set; }
    }

    public class NotifySendResp : Resp
    {
        /// <summary>
        ///  消息处理业务Id
        /// </summary>
        public string msg_biz_id { get; set; } = string.Empty;
    }

}
