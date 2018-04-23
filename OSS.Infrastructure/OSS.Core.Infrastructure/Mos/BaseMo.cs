#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 实体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion

namespace OSS.Core.Infrastructure.Mos
{
    public class BaseMo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        public int status { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public long create_time { get; set; }
    }


    public class BaseUIdMo:BaseMo
    {
        /// <summary>
        ///  用户id
        /// </summary>
        public long u_id { get; set; }
    }
}