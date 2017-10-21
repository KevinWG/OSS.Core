
#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：基础类库 - 短信插件接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com       
*    	创建日期： 2017-5-31
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;

namespace OSS.Core.Infrastructure.Plugs
{
  
    /// <summary>
    ///  短信插件接口
    /// </summary>
    public interface ISmsPlug 
    {
        /// <summary>
        ///  发送短信
        /// </summary>
        /// <param name="receiveNum"></param>
        /// <param name="msgId"></param>
        /// <param name="contentData"></param>
        /// <returns></returns>
        Task<ResultMo> SendMsg(string receiveNum, string msgId, Dictionary<string, string> contentData);
    }

    /// <summary>
    ///  邮件插件接口
    /// </summary>
    public interface IEmailPlug 
    {
        /// <summary>
        ///  发送短信
        /// </summary>
        /// <param name="receiveNum"></param>
        /// <param name="msgId"></param>
        /// <param name="contentData"></param>
        /// <param name="attach"></param>
        /// <returns></returns>
        Task<ResultMo> SendMsg(string receiveNum, string msgId, Dictionary<string, string> contentData, object attach);
    }
}
