
#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：基础类库 - 短信消息实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com       
*    	创建日期： 2017-11-9
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;

namespace OSS.Core.Services.Plugs.Notify.EmailImp.Mos
{
    /// <summary>
    ///  邮件消息实体
    /// </summary>
    public class EmailMsgMo
    {
        /// <summary>
        ///  发送的邮件对象
        /// </summary>
        public IList<string> to_emails { get; set; }

        /// <summary>
        ///  是否是Html
        /// </summary>
        public bool is_html { get; set; }

        /// <summary>
        ///  邮件内容
        /// </summary>
        public string body { get; set; }

        /// <summary>
        ///  邮件主题
        /// </summary>
        public string subject { get; set; }

    }

}
