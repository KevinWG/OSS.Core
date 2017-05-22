using System.Threading.Tasks;
using Dapper;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;

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


        /// <summary>
        ///   判断账号是否可用于注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ResultMo> CheckIfCanRegiste(RegLoginType type, string value)
        {
            var sql =
                $"select count(1) from {m_TableName} where {(type == RegLoginType.Email ? "emial " : "mobile ")} =@val";
            return await ExcuteReadeResAsync(async con =>
            {
                var count =await con.ExecuteScalarAsync<int>(sql, new {val = value});
                return count > 0 ? new ResultMo(ResultTypes.ObjectExsit, "账号已存在，无法注册！") : new ResultMo();
            });
        }
    }
}
