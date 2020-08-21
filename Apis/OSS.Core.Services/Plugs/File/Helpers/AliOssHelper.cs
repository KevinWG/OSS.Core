using System;
using System.Text;
using Aliyun.OSS;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Encrypt;
using OSS.Common.Extention;
using OSS.Core.Context;
using OSS.Core.Infrastructure.BasicMos.File;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.RepDapper.Plugs.File.Mos;
using OSS.Tools.Config;

namespace OSS.Core.Services.Plugs.File.Helpers
{
    internal static class AliOssHelper
    {
        private static readonly AppConfig _aliConfig = new AppConfig()
            {
                AppId = ConfigHelper.GetSection("Service:Ali:OSS:OssKey")?.Value,
                AppSecret = ConfigHelper.GetSection("Service:Ali:OSS:OssSecret")?.Value
            };

        /// <summary>
        ///  应用内部Token加密秘钥
        /// </summary>
        public static string OssTokenSecret { get; } = ConfigHelper.GetSection("Service:Ali:OSS:OssTokenSecret")?.Value;

        /// <summary>
        ///   获取上传参数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileCategory">文件分类</param>
        /// <returns></returns>
        public static Resp<BucketUploadPara> GetUploadPara(string userId, string fileCategory)
        {
            return GetUploadPara("qb-thums", fileCategory, userId);
        }

        /// <summary>
        /// 获取上传参数
        /// </summary>
        /// <param name="bucket_name"></param>
        /// <param name="category"></param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        private static Resp<BucketUploadPara> GetUploadPara(string bucket_name, string category, string userId)
        {
            var osappInfo = AppReqContext.Identity;

            var domain = GetDomainByBucket(bucket_name);
            if (string.IsNullOrEmpty(domain))
                return new Resp<BucketUploadPara>().WithResp(RespTypes.NoPermission, "没有当前图片空间的上传权限");

            var patStr = new StringBuilder();
            if (AppInfoHelper.IsDev)
                patStr.Append("test/");

            patStr.Append(osappInfo.tenant_id).Append("/")
                .Append((int)osappInfo.app_type).Append("-").Append(userId).Append("/")
                .Append(category).Append("/")
                .Append(DateTime.Now.ToUtcMilliSeconds());

            var key = patStr.ToString();


            var expiration = DateTime.Now.AddMinutes(3);

            //var callBack = GetCallBack(key);
            var policyConds = new PolicyConditions();

            policyConds.AddConditionItem("bucket", bucket_name);
            policyConds.AddConditionItem(MatchMode.StartWith, PolicyConditions.CondKey, key);
            //policyConds.AddConditionItem("callback", callBack);

            var encPostPolicy = policyConds.GeneratePostPolicyJsonBase64(expiration);
            var signature = HMACSHA.EncryptBase64(encPostPolicy, _aliConfig.AppSecret);
            var bucketConfig = new BucketUploadPara();

            bucketConfig.paras.Add("key", key);
            bucketConfig.paras.Add("OSSAccessKeyId", _aliConfig.AppId);
            bucketConfig.paras.Add("policy", encPostPolicy);
            bucketConfig.paras.Add("signature", signature);
            //bucketConfig.paras.Add("callback", callBack);

            bucketConfig.upload_url = domain;

            return new Resp<BucketUploadPara>(bucketConfig);
        }

        private static string _aliCallBackUrl = ConfigHelper.GetSection("Service:Ali:OSS:OssCallBack")?.Value;

        private static string GetCallBack(string key)
        {
            var token = HMACSHA.EncryptBase64(key, OssTokenSecret);

            var callBackStr = new StringBuilder();
            callBackStr.Append("{\"callbackUrl\":").Append("\"").Append(_aliCallBackUrl).Append("?t=").Append(token.UrlEncode()).Append("\",");
            //callBackStr.Append("\"callbackBodyType\":").Append("\"application/json\",");
            //callBackStr.Append("\"callbackBody\":").Append("\"{\"mime_type\":${mimeType},\"size\":${size},\"object\":${object},\"bucket\":${bucket}}\"}");
            callBackStr.Append("\"callbackBody\":").Append("bucket=${bucket}&object=${object}&size=${size}&mimeType=${mimeType}}");

            return callBackStr.ToString().ToBase64(Encoding.UTF8);
        }

        private static string GetDomainByBucket(string bucket_name)
        {
            switch (bucket_name)
            {
                case "qb-thums":
                    return "http://img1.osscore.cn/";
            }
            return string.Empty;
        }


        /// <summary>
        ///  格式化 上传文件实体
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="token"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool FormatUploadMo(UploadFileMo mo, string token, string key)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            var encryptKey = HMACSHA.EncryptBase64(key, OssTokenSecret);
            if (token != encryptKey)
                return false;

            var splitKey = key.Split('/');
            var length = splitKey.Length;
            if (length < 4)
                return false;

            var splitUser = splitKey[length - 3].Split('-');
            if (splitUser.Length < 2)
                return false;

            mo.owner_tid = splitKey[length - 4];

            //var appType = splitUser[0].ToInt32();
            mo.owner_tid = mo.owner_tid;
            mo.owner_uid = splitUser[1];
            mo.url = string.Concat(GetDomainByBucket(mo.bucket), "/", key);
            if (mo.mime_type.StartsWith("image"))
            {
                mo.type = UploadFileType.Image;
            }

            return true;
        }
    }



 
}
