#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore基础层 ——  短信插件实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-7
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using OSS.Clients.SMS.Ali.Reqs;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Services.Plugs.Notify.Mos
{
    public enum NotifyType
    {
        Email   = 10,
        SMS = 20,
        AppMsg = 30
    }
    public enum NotifyChannel
    {
        Self,
        AliYun,
    }

    public class NotifyReq
    {
        /// <summary>
        /// 目标账号
        /// </summary>
        public IList<string> targets { get; set; }

        /// <summary>
        ///  模板编号
        /// </summary>
        public string t_code { get; set; }

        /// <summary>
        ///  消息Id
        /// </summary>
        public string msg_Id { get; set; }

        /// <summary>
        ///  消息标题
        ///    如若为空，取模板的标题
        /// </summary>
        public string msg_title { get; set; }

        /// <summary>
        ///  内容数据
        /// </summary>
        public Dictionary<string, string> body_paras { get; set; }
    }



    public class NotifyResp : Resp
    {
        /// <summary>
        ///  消息处理业务Id
        /// </summary>
        public string msg_biz_id { get; set; }
    }

    public static class NotifyRespMaps
    {
        public static NotifyResp ToNotifyResp(this SendAliSmsResp aRes)
        {
            var resp = new NotifyResp();
            resp.WithResp(aRes);

            resp.msg_biz_id = aRes.BizId;

            return resp;
        }
    }
}
