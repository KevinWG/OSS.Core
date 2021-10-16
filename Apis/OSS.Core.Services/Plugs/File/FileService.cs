using System;
using System.IO;
using System.Text;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extension;
using OSS.Core.Context;
using OSS.Core.Infrastructure;
using OSS.Core.Services.Plugs.File.Reqs;

namespace OSS.Core.Services.Plugs.File
{
    public class FileService
    {
        #region 获取上传参数

        /// <summary>
        ///  获取商品上传的参数
        /// </summary>
        /// <returns></returns>
        public Resp<BucketUploadPara> GetImgUploadParas(string name)
        {
            return GetUploadParas("imgs", name);
        }


        public Resp<BucketUploadPara> GetFileUploadParas(string name)
        {
            return GetUploadParas("files", name);
        }

        #endregion


        /// <summary>
        /// 直接上传
        /// </summary>
        /// <returns></returns>
        public Resp Upload(UploadFileReq req, Stream file)
        {
            return new Resp(RespTypes.OperateFailed, "未实现该功能！");
        }

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
        private static Resp<BucketUploadPara> GetUploadParas(string category, string fileName)
        {
            var path = GetPathKey(category, fileName, CoreUserContext.Identity.id);
            return new Resp<BucketUploadPara>().WithResp(RespTypes.OperateFailed, "未实现该功能！");
        }


        private static string GetPathKey(string category,string fileName, string userId)
        {
            //var appInfo = AppReqContext.Identity;

            var patStr = new StringBuilder();
            if (AppInfoHelper.IsDev)
                patStr.Append("test/");

            patStr.Append(category).Append("/")
                //.Append(appInfo.tenant_id).Append("/")
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
