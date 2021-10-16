#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 后台用户仓储实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure;
using OSS.Core.RepDapper.Basic.Portal.Mos;
using UserStatus = OSS.Core.RepDapper.Basic.Portal.Mos.UserStatus;

namespace OSS.Core.RepDapper.Basic.Portal
{
    public class UserInfoRep : BaseRep<UserInfoRep,UserInfoBigMo>
    {
        protected override string GetTableName()
        {
            return "b_portal_user";
        }
     
      

        /// <summary>
        ///  获取平台列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Task<PageListResp<UserInfoBigMo>> SearchUsers(SearchReq search)
        {
            return SimpleSearch(search);
        }


        protected override string BuildSimpleSearchWhereSqlByFilterItem(string key, string value, Dictionary<string, object> sqlParas)
        {
            switch (key)
            {
                case "email":
                    sqlParas.Add("@email",value);
                    return "t.email=@email";
            }
            return base.BuildSimpleSearchWhereSqlByFilterItem(key, value, sqlParas);
        }

        public async Task<Resp<UserInfoBigMo>> GetUserByLoginType(string name, RegLoginType type)
        {
            return await (type == RegLoginType.Mobile
                ? Get(u => u.mobile == name  && u.status >= UserStatus.Locked)
                : Get(u => u.email == name  && u.status >= UserStatus.Locked));
        }

        public override Task<Resp<UserInfoBigMo>> GetById(long id)
        {
            Func<Task<Resp<UserInfoBigMo>>> getFunc = () => base.GetById(id);
            var userKey = string.Concat(CoreCacheKeys.Portal_User_ById, id);

            return getFunc.WithCache(userKey, TimeSpan.FromHours(1));
        }
        

        /// <summary>
        ///  修改用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Task<Resp> UpdateStatus(long id, UserStatus state)
        {
            var userKey = string.Concat(CoreCacheKeys.Portal_User_ById, id);
            return Update(t => new { status = state }, t => t.id == id )
                .WithCacheClear(userKey);
        }

    }
}
