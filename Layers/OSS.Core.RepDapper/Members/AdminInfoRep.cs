#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 后台管理员仓储实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion


using OSS.Core.Domains.Members.Interfaces;

namespace OSS.Core.RepDapper.Members
{
    /// <summary>
    ///  后台管理员仓储实现
    /// </summary>
    public class AdminInfoRep:BaseRep,IAdminInfoRep
    {
        public AdminInfoRep()
        {
            m_TableName = "admin_info";
        }
    }
}
