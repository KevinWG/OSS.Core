using System.ComponentModel.DataAnnotations;
using OSS.Core.Infrastructure.BasicMos.Enums;
using OSS.Core.RepDapper.Basic.Portal.Mos;

namespace OSS.Core.WebApi.Controllers.Basic.Portal.Reqs
{
 
    public class AddUserReq
    {
        [DataType(DataType.PhoneNumber,ErrorMessage = "请填写正确的手机号码")]
        public string mobile { get; set; }
        
        [DataType(DataType.EmailAddress,ErrorMessage = "请填写正确的邮箱地址")]
        public string email { get; set; }


        [Required(ErrorMessage = "用户昵称不能为空")]
        public string nick_name { get; set; }
    }

    public static class AddUserReqMap
    {
        public static UserInfoBigMo MapToUserInfo(this AddUserReq req)
        {
            var userInfo = new UserInfoBigMo
            {
                mobile    = req.mobile,
                nick_name = req.nick_name,
                email     = req.email
            };
            return userInfo;
        }
    }



    public class CheckRegNameReq
    {
        /// <summary>
        ///  注册类型
        /// </summary>
        public RegLoginType reg_type { get; set; }

        /// <summary>
        ///  注册 手机号/邮箱 名称
        /// </summary>
       [Required(ErrorMessage = "手机号/邮箱 不能为空")] public string reg_name { get; set; }
    }

}
