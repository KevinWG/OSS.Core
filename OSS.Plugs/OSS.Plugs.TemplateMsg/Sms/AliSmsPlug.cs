#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore插件 —— 阿里云 短信实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-22
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Plugs;

namespace OSS.Plugs.TemplateMsg.Sms
{
    /// <summary>
    ///  阿里云的短信实现
    /// </summary>
    public class AliSmsPlug:ISmsPlug
    {
        public Task<ResultMo> SendMsg(string receiveNum, string templateId, Dictionary<string, string> contentData)
        {
            throw new System.NotImplementedException();
        }
    }
}
