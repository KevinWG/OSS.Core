#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 用户实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion

namespace OSS.Core.Module.Portal;


public class UserInfoMo : UserBasicMo
{
    /// <summary>
    /// 密码
    /// </summary>
    public string? pass_word { get; set; }
}


public static class UserInfoMoMaps
{
    /// <summary>
    ///  BigMo转化为Mo
    ///    主要防止直接返回BigMo附带用户密码，来源渠道等字段
    /// </summary>
    /// <param name="io"></param>
    /// <returns></returns>
    public static UserBasicMo ConvertToMo(this UserInfoMo io)
    {
        var userInfo = new UserBasicMo
        {
            email  = io.email,
            avatar = io.avatar,
            mobile = io.mobile,
            id     = io.id,

            nick_name = io.nick_name,
            add_time  = io.add_time,
            status    = io.status,
            //from_app_id = io.from_app_id,
        };
        return userInfo;
    }

    public static UserInfoMo MapToUserInfo(this AddUserReq req)
    {
        var userInfo = new UserInfoMo
        {
            mobile    = req.mobile,
            nick_name = req.nick_name,
            email     = req.email
        };
        return userInfo;
    }

}