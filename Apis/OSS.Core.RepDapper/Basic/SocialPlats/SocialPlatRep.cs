using System.Threading.Tasks;
using OSS.Common.Resp;
using OSS.Core.RepDapper.Basic.SocialPlats.Mos;

namespace OSS.Core.RepDapper.Basic.SocialPlats
{
    public class SocialPlatRep:BaseRep<SocialPlatRep, SocialPlatformMo>
    {
        protected override string GetTableName()
        {
            return "n_social_platforms";
        }

        /// <summary>
        /// 获取绑定的社交平台信息
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        public  async Task<Resp<long>> AddSocialPlatform(SocialPlatformMo mo)
        {
            var socialRes = await Get(s =>  s.social_plat == mo.social_plat);
            if (socialRes.IsSuccessOrDataNull())
                return await Add(mo);

            if (!socialRes.IsSuccess())
                return new Resp<long>().WithResp(socialRes);// socialRes.ConvertToResultInherit<IdResp>();

            var updateRes = await Update(m => new {m.status, m.name}, s => s.id == mo.id, mo);
            return updateRes.IsSuccess() ? new Resp<long>(mo.id) : new Resp<long>().WithResp(updateRes);// updateRes.ConvertToResultInherit<IdResp>();

        }
    }
}
