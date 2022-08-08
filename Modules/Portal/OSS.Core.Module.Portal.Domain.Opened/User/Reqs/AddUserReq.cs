using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Portal
{
    public class AddUserReq
    {
        [DataType(DataType.PhoneNumber, ErrorMessage = "请填写正确的手机号码")]
        public string? mobile { get; set; } 

        [DataType(DataType.EmailAddress, ErrorMessage = "请填写正确的邮箱地址")]
        public string? email { get; set; }

        [Required(ErrorMessage = "用户昵称不能为空")]
        public string nick_name { get; set; } = string.Empty;
    }

    public class UpdateUserBasicReq
    {
        public string? avatar { get; set; }

        [Required(ErrorMessage = "用户昵称不能为空")] public string nick_name { get; set; } = string.Empty;
    }
    
}
