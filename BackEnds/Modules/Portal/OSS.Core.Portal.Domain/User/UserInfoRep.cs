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

using OSS.Common.Resp;
using OSS.Common;
using OSS.Core.Extension;
using OSS.Core.Portal.Domain;
using OSS.Core.Rep.Mysql;
using OSS.Tools.Config;

namespace OSS.Core.Reps.Basic.Portal
{
    public class UserInfoRep : BaseMysqlRep<UserInfoRep, UserInfoMo,long>
    {
        private static readonly string _writeConnection = ConfigHelper.GetConnectionString("WriteConnection");
        private static readonly string _readConnection = ConfigHelper.GetConnectionString("ReadConnection");

        public UserInfoRep() : base(_writeConnection, _readConnection)
        {
        }

        protected override string GetTableName()
        {
            return "b_portal_user";
        }

        /// <summary>
        ///  获取平台列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Task<PageListResp<UserInfoMo>> SearchUsers(SearchReq search)
        {
            return SimpleSearch(search);
        }


        protected override string BuildSimpleSearch_FilterItemSql(string key, string value,
            Dictionary<string, object> sqlParas)
        {
            switch (key)
            {
                case "email":
                    sqlParas.Add("@email", value);
                    return "t.email=@email";
                case "mobile":
                    sqlParas.Add("@mobile", value);
                    return "t.mobile=@mobile";
            }

            return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
        }




        /// <summary>
        ///  获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<Resp<UserInfoMo>> GetUserByLoginType(string name, PortalCodeType type)
        {
            return type == PortalCodeType.Mobile
                ? Get(u => u.mobile == name && u.status >= UserStatus.Locked)
                : Get(u => u.email == name && u.status >= UserStatus.Locked);
        }




        /// <summary>
        ///  修改用户登录信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Resp> UpdatePortalByType(long id, PortalCodeType type, string name)
        {
            var userKey = string.Concat(PortalConst.CacheKeys.Portal_User_ById, id);
            return (
                type == PortalCodeType.Mobile
                    ? Update(t => new { mobile = name }, t => t.id == id)
                    : Update(t => new { email = name }, t => t.id == id)
            ).WithCacheClear(userKey);
        }


        public override Task<Resp<UserInfoMo>> GetById(long id)
        {
            Func<Task<Resp<UserInfoMo>>> getFunc =
                () => base.GetById(id);

            var userKey = string.Concat(PortalConst.CacheKeys.Portal_User_ById, id);

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
            var userKey = string.Concat(PortalConst.CacheKeys.Portal_User_ById, id);
            return Update(t => new { status = state }, t => t.id == id)
                .WithCacheClear(userKey);
        }


        /// <summary>
        ///  修改基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="avatar"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public Task<Resp> UpdateBasicInfo(long id, string avatar, string nickName)
        {
            var userKey = string.Concat(PortalConst.CacheKeys.Portal_User_ById, id);
            return Update(t => new { avatar = avatar, nick_name = nickName }, t => t.id == id)
                .WithCacheClear(userKey);
        }


     
    }
}
