using System;
using System.Text;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extension;
using OSS.Core.Context;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Services.Plugs.File.Reqs;

namespace OSS.Core.Services.Plugs.File
{
    public class FileService
    {

        #region 获取上传参数

        /// <summary>
        ///  获取上传头像的参数
        /// </summary>
        /// <returns></returns>

        public Resp<BucketUploadPara> AvatarUploadPara(string name)
        {
            return GetUploadPara("avatar", name);
        }

        /// <summary>
        ///  获取商品上传的参数
        /// </summary>
        /// <returns></returns>

        public Resp<BucketUploadPara> GetGoodsUploadParas(string name)
        {
            return GetUploadPara("goods", name);
        }

        /// <summary>
        ///  获取商品上传的参数
        /// </summary>
        /// <returns></returns>

        public Resp<BucketUploadPara> GetEditorUploadParas(string name)
        {
            return GetUploadPara("rich_editor", name);
        }

        /// <summary>
        ///  获取商品上传的参数
        /// </summary>
        /// <returns></returns>

        public Resp<BucketUploadPara> GetConfigUploadParas(string name)
        {
            return GetUploadPara("config", name);
        }
        #endregion



        ///// <summary>
        /////  获取用户图片列表
        ///// </summary>
        ///// <param name="search"></param>
        ///// <returns></returns>
        //public async Task<PageListResp<UploadFileMo>> GetUploadImgs(SearchReq search)
        //{
        //    search.filter.Add("owner_uid", UserContext.Identity.id);
        //    search.filter.Add("file_type", ((int)UploadFileType.Image).ToString());

        //    return await UploadFileRep.Instance.GetPageList(search);
        //}



        /// <summary>
        ///  获取上传参数信息
        /// </summary>
        /// <returns></returns>
        private static Resp<BucketUploadPara> GetUploadPara(string category, string fileName)
        {
            var path = GetPathKey(category, fileName, UserContext.Identity.id);
            return new Resp<BucketUploadPara>().WithResp(RespTypes.OperateFailed, "未实现该功能");
        }


        private static string GetPathKey(string category,string fileName, string userId)
        {
            var appInfo = AppReqContext.Identity;

            var patStr = new StringBuilder();
            if (AppInfoHelper.IsDev)
                patStr.Append("test/");

            patStr
                //.Append(appInfo.tenant_id).Append("/")
                //.Append((int)appInfo.app_type)
                .Append(category).Append("/")
                .Append(DateTime.Now.ToString("yyyy-MM")).Append("/")
                
                .Append(userId).Append("/")
                .Append(DateTime.Now.ToUtcMilliSeconds());

            int index;
            if (!string.IsNullOrEmpty(fileName)&&(index= fileName.LastIndexOf('.'))>=0)
            {
                patStr.Append(fileName.Substring(index));
            }

            return patStr.ToString();
        }
    }
}
