#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore插件 —— 邮件的插件实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-22
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Plugs;

namespace OSS.Plugs.TemplateMsg.Email
{
    /// <summary>
    ///  邮件的插件实现
    /// </summary>
    public class EmailPlug:IEmailPlug
    {
        public Task<ResultMo> SendMsg(string receiveNum, string templateId, Dictionary<string, string> contentData)
        {
            throw new NotImplementedException();
        }
    }
}
