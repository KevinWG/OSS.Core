
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 管理员实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion

namespace OSS.Core.DomainMos.Members.Mos
{
    /// <summary>
    ///  后台管理员实体
    /// </summary>
    public class AdminInfoMo:BaseAutoMo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long uid { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        public string name { get; set; }

    }

}
