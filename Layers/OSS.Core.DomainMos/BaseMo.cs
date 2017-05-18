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

using OSS.Common.Extention.DTO;

namespace OSS.Core.DomainMos
{
    public class BaseMo
    {
        /// <summary>
        /// 状态信息
        /// </summary>
        public int status { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public long create_time { get; set; }
    }

    
    public class BaseAutoMo: BaseMo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [AutoColumn]
        public long Id { get; set; }
    }

    public class BaseMo<MoType>: BaseMo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public MoType Id { get; set; }
    }

  
}