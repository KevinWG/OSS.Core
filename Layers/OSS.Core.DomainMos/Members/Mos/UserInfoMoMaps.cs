namespace OSS.Core.Domains.Members.Mos
{
    public static class UserInfoMoMaps
    {
        /// <summary>
        ///  BigMo转化为Mo
        ///    主要防止直接返回BigMo附带用户密码，来源渠道等字段
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        public static UserInfoMo ConvertToMo(this UserInfoBigMo io)
        {
            var userInfo = new UserInfoMo
            {
                email = io.email,
                nick_name = io.nick_name,
                mobile = io.mobile,
                Id = io.Id,
                create_time = io.create_time,

                status = io.status
            };
            return userInfo;
        }
    }
}
