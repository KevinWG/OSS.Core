using System;
using OSS.Common.ComModels;

#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 后台用户仓储实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion
using OSS.Core.DomainMos.Members.Interfaces;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.RepDapper.Members
{
    public class UserInfoRep : BaseRep, IUserInfoRep
    {
        public UserInfoRep()
        {
            m_TableName = "user_info";
        }

        public ResultMo CheckIfCanRegiste(RegLoginType type, string value)
        {
            throw new NotImplementedException();
        }
    }
}
