
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
    public interface ITmsPlug
    {
        /// <summary>
        ///  发送固定模板消息
        /// </summary>
        /// <param name="receiveNum">接收账号</param>
        /// <param name="templateId">模板Id</param>
        /// <param name="contentData">内容数据</param>
        /// <returns></returns>
        Task<ResultMo> SendMsg(string receiveNum, string templateId, Dictionary<string, string> contentData);
    }

    /// <summary>
    ///  短信插件接口
    /// </summary>
    public interface ISmsPlug : ITmsPlug
    {
    }

    /// <summary>
    ///  邮件插件接口
    /// </summary>
    public interface IEmailPlug : ITmsPlug
    {
    }
    
}
