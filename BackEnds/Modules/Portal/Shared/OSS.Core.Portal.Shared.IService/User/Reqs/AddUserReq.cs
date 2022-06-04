using System.ComponentModel.DataAnnotations;
using OSS.Core.Portal.Domain;

namespace OSS.Core.Services.Basic.Portal.Reqs
{
    public class AddUserReq
    {
        [DataType(DataType.PhoneNumber, ErrorMessage = "请填写正确的手机号码")]
        public string mobile { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "请填写正确的邮箱地址")]
        public string email { get; set; }

        [Required(ErrorMessage = "用户昵称不能为空")]
        public string nick_name { get; set; }
    }

    public static class AddUserReqMap
    {
        public static UserInfoMo MapToUserInfo(this AddUserReq req)
        {
            var userInfo = new UserInfoMo
            {
                mobile = req.mobile,
                nick_name = req.nick_name,
                email = req.email
            };
            return userInfo;
        }
    }


    public class UpdateUserBasicReq
    {
        public string avatar { get; set; }

        [Required(ErrorMessage = "用户昵称不能为空")]
        public string nick_name { get; set; }
    }





}
