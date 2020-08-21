using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Enums;
using OSS.Common.Extention;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.BasicMos.File;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Web.Attributes.Auth;

using OSS.Core.RepDapper.Plugs.File.Mos;
using OSS.Core.Services.Plugs.File;

namespace OSS.Core.WebApi.Controllers.Plugs.File
{
    [ModuleName(ModuleNames.File)]
    [Route("p/[controller]/[action]/{id?}")]
    public class FileController : BaseController
    {
        private static readonly FileService _service = new FileService();
        
        /// <summary>
        ///  获取用户上传的图片列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageListResp<UploadFileMo>> GetUploadImgs([FromBody] SearchReq req)
        {
            if (req == null)
                return new PageListResp<UploadFileMo>().WithResp(RespTypes.ParaError, "参数错误！");
            return await _service.GetUploadImgs(req);
        }
        
        /// <summary>
        ///  获取上传所需相关参数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Resp<BucketUploadPara>> GetUploadParas(string category)
        {
            if (string.IsNullOrEmpty(category))
                return new Resp<BucketUploadPara>().WithResp(RespTypes.ParaError, "category 参数错误！");

            return await _service.GetUploadPara(category);
        }
    }

    //
    public class UploadNotifyController : BasePartnerController
    {
        //  ali
        //public 
        private static readonly FileService _service = new FileService();
        
        [HttpPost]
        [AppPartnerName("AliOSS")]
        public async Task<Resp> ali([FromQuery] string t)
        {
            var token =  t;

            var size = Request.Form["size"].FirstOrDefault();
            var bucket = Request.Form["bucket"].FirstOrDefault();
            var key = Request.Form["object"].FirstOrDefault();
            var mime_type = Request.Form["mimeType"].FirstOrDefault();

            var mo = new UploadFileMo
            {
                mime_type = mime_type,
                size = size.ToInt32(),
                bucket = bucket,
                status = CommonStatus.Original
            };
            
            return await _service.UploadNotify(mo, token, key);
        }

    }
}