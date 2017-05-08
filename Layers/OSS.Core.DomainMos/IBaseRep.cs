#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore领域模型层 —— 仓储基类接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion

using System;
using System.Linq.Expressions;
using OSS.Common.ComModels;

namespace OSS.Core.DomainMos
{
    public interface IBaseRep
    {
        /// <summary>
        ///   插入数据（默认Id自增长
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="isAuto">Id主键是否自增长</param>
        /// <returns></returns>
        ResultIdMo Insert<T>(T mo,bool isAuto=true);

        /// <summary>
        /// 全量更新
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        ResultMo UpdateAll<TType>(TType mo, Expression<Func<TType, bool>> whereExp = null);

        /// <summary>
        /// 部分字段的更新
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="updateExp">更新字段new{m.Name,....} Or new{m.Name="",....}</param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        ResultMo Update<TType>(TType mo, Expression<Func<TType, object>> updateExp,
            Expression<Func<TType, bool>> whereExp = null);

        /// <summary>
        /// 软删除，仅仅修改State状态
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        ResultMo DeleteSoft<TType>(TType mo, Expression<Func<TType, bool>> whereExp = null) where TType : BaseMo;

        /// <summary>
        ///  获取单个实体对象
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        ResultMo<TType> Get<TType>(TType mo, Expression<Func<TType, bool>> whereExp = null);
    }
}
