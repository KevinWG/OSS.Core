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

namespace OSS.Core.DomainMos
{
    public class BaseMo<IdType>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        private IdType Id { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public long create_time { get; set; }
    }
}