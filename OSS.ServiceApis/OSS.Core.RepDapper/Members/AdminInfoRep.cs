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


using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Core.Domains.Members;
using OSS.Plugs.OrmMysql;

namespace OSS.Core.RepDapper.Members
{
    /// <summary>
    ///  后台管理员仓储实现
    /// </summary>
    public class AdminInfoRep: BaseMysqlRep<AdminInfoRep,AdminInfoMo>
    {
        public AdminInfoRep()
        {
            m_TableName = "admin_info";
        }


        public async Task<ResultMo<AdminInfoMo>> GetById(long id)
        {
            return await GetById(id);
        }
    }
}
