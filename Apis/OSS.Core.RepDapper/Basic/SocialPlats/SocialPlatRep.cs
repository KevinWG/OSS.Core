//using System.Threading.Tasks;
//using OSS.Common.BasicMos.Resp;
//using OSS.Core.RepDapper.Basic.SocialPlats.Mos;

//namespace OSS.Core.RepDapper.Basic.SocialPlats
//{
//    public class SocialPlatRep:BaseTenantRep<SocialPlatRep, SocialPlatformMo>
//    {
//        protected override string GetTableName()
//        {
//            return "n_social_platforms";
//        }

//        /// <summary>
//        /// 获取绑定的社交平台信息
//        /// </summary>
//        /// <param name="mo"></param>
//        /// <returns></returns>
//        public  async Task<IdResp<string>> AddSocialPlatform(SocialPlatformMo mo)
//        {
//            var socialRes = await Get(s => s.owner_tid == mo.owner_tid && s.social_plat == mo.social_plat);
//            if (socialRes.IsRespType(RespTypes.ObjectNull))
//                return await Add(mo);

//            if (!socialRes.IsSuccess())
//                return new IdResp<string>().WithResp(socialRes);// socialRes.ConvertToResultInherit<IdResp>();

//            var updateRes = await Update(m => new {m.status, m.name}, s => s.id == mo.id, mo);
//            return updateRes.IsSuccess() ? new IdResp(mo.id) : new IdResp<string>().WithResp(updateRes);// updateRes.ConvertToResultInherit<IdResp>();

//        }
//    }
//}
