#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore领域模型层 —— 用户仓储接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.Domains.Members.Interfaces
{
    public interface IUserInfoRep: IBaseRep
    {
        /// <summary>
        /// 判断是否账号是否可以注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<ResultMo> CheckIfCanRegiste(RegLoginType type, string value);

        /// <summary>
        ///  获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ResultMo<UserInfoBigMo>> GetUserByLoginType(string name, RegLoginType type);
    }
}
