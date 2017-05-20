#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore仓储层 —— 仓储基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using MySql.Data.MySqlClient;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.DomainMos;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.RepDapper.OrmExtention;

namespace OSS.Core.RepDapper
{
    /// <summary>
    /// 仓储层基类
    /// </summary>
    public class BaseRep
    {
        protected string m_TableName;

        protected readonly string writeConnectionString;
        protected readonly string readeConnectionString;

        public BaseRep(string writeConnectionStr = null, string readeConnectionStr = null)
        {
            writeConnectionString = writeConnectionStr ?? ConfigUtil.GetConnectionString("WriteConnection");
            readeConnectionString = readeConnectionStr ?? ConfigUtil.GetConnectionString("ReadeConnection");
        }

        #region 底层基础读写分离封装

        /// <summary>
        /// 执行写数据库操作
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal RType ExcuteWrite<RType>(Func<IDbConnection, RType> func) where RType : ResultMo, new()
            => Execute(func, true);

        /// <summary>
        ///  执行读操作，返回具体类型，自动包装成ResultMo结果实体
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal ResultMo<RType> ExcuteReade<RType>(Func<IDbConnection, RType> func) => Execute(con =>
        {
            var res = func(con);
            return res != null ? new ResultMo<RType>(res) : new ResultMo<RType>(ResultTypes.ObjectNull, "未发现相关数据！");
        }, false);

        /// <summary>
        /// 执行读操作，直接返回继承自ResultMo实体
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal RType ExcuteReadeRes<RType>(Func<IDbConnection, RType> func) where RType : ResultMo, new()
            => Execute(func, false);

        private RType Execute<RType>(Func<IDbConnection, RType> func, bool isWrite)
            where RType : ResultMo, new()
        {
            var t = default(RType);
            try
            {
                using (var con = new MySqlConnection(isWrite ? writeConnectionString : readeConnectionString))
                {
                    t = func(con);
                }
            }
            catch (Exception e)
            {
               #if DEBUG
                throw e;
               #else
                 LogUtil.Error(string.Concat("数据库操作错误，详情：", e.Message, "\r\n", e.StackTrace), "DataRepConnectionError",
                    "DapperRep");
                t = new RType
                {
                    Ret = (int) ResultTypes.InnerError,
                    Message = "数据更新出错！"
                };
                return t;
               #endif
            }
            return t ?? new RType() {Ret = (int) ResultTypes.ObjectNull, Message = "未发现对应结果响应"};
        }

        #endregion

        #region   基础CRUD操作方法

        /// <summary>
        ///   插入数据（默认Id自增长
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="isAuto">Id主键是否自增长</param>
        /// <returns></returns>
        public virtual ResultIdMo Insert<T>(T mo, bool isAuto = true)
            => ExcuteWrite(con => con.Insert(mo, isAuto, m_TableName));

        /// <summary>
        /// 全量更新
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        public virtual ResultMo UpdateAll<TType>(TType mo, Expression<Func<TType, bool>> whereExp = null)
            => ExcuteWrite(con => con.UpdateAll(mo, whereExp, m_TableName));


        /// <summary>
        /// 部分字段的更新
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="updateExp">更新字段new{m.Name,....} Or new{m.Name="",....}</param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        public virtual ResultMo Update<TType>(TType mo, Expression<Func<TType, object>> updateExp,
            Expression<Func<TType, bool>> whereExp = null)
            => ExcuteWrite(con => con.UpdatePartail(mo, updateExp, whereExp, m_TableName));

        /// <summary>
        /// 软删除，仅仅修改State状态
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        public virtual ResultMo DeleteSoft<TType>( Expression<Func<TType, bool>> whereExp = null, TType mo=null)
            where TType : BaseMo
        {
            mo.status = (int) CommonStatus.Delete;
            return ExcuteWrite(con => con.UpdatePartail(mo, m => new {m.status }, whereExp, m_TableName));
        }

        /// <summary>
        ///  获取单个实体对象
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        public virtual ResultMo<TType> Get<TType>(Expression<Func<TType, bool>> whereExp = null, TType mo = null) where TType : class
            => ExcuteReade(con => con.Get(mo, whereExp, m_TableName));



        /// <summary>
        ///   列表查询
        /// </summary>
        /// <param name="selectSql">查询语句，包含排序等</param>
        /// <param name="totalSql">查询数量语句，不需要排序</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected internal PageListMo<TType> GetList<TType>(string selectSql, string totalSql,
            Dictionary<string, object> paras)
            where TType : ResultMo, new()
        {
            return ExcuteReadeRes(con =>
            {
                var para = new DynamicParameters(paras);
                var total = con.ExecuteScalar<long>(totalSql, para);
                var list = con.Query<TType>(selectSql, para).ToList();
                return new PageListMo<TType>(total, list);
            });
        }

        #endregion

    }



}






