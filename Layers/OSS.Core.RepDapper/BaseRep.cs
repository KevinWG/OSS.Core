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
using System.Threading.Tasks;
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
        protected internal async Task<RType> ExcuteWriteAsync<RType>(Func<IDbConnection, Task<RType>> func) where RType : ResultMo, new()
            =>await Execute(func, writeConnectionString);

        /// <summary>
        ///  执行读操作，返回具体类型，自动包装成ResultMo结果实体
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal async Task<ResultMo<RType>> ExcuteReadeAsync<RType>(Func<IDbConnection, Task<RType>> func) =>await Execute(async con =>
        {
            var res =await func(con);
            return res != null ? new ResultMo<RType>(res) : new ResultMo<RType>(ResultTypes.ObjectNull, "未发现相关数据！");
        }, readeConnectionString);

        /// <summary>
        /// 执行读操作，直接返回继承自ResultMo实体
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal async Task<RType> ExcuteReadeResAsync<RType>(Func<IDbConnection, Task<RType>> func) where RType : ResultMo, new()
            =>await Execute(func, readeConnectionString);

        private static async Task<RType> Execute<RType>(Func<IDbConnection,Task<RType>> func, string connecStr)
            where RType : ResultMo, new()
        {
            var t = default(RType);
            try
            {
                using (var con = new MySqlConnection(connecStr))
                {
                    t =await func(con);
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
        public virtual async Task<ResultIdMo> Insert<T>(T mo, bool isAuto = true)
            =>await ExcuteWriteAsync(con => con.Insert(mo, isAuto, m_TableName));

        /// <summary>
        /// 全量更新
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        public virtual async Task<ResultMo> UpdateAll<TType>(TType mo, Expression<Func<TType, bool>> whereExp = null)
            => await ExcuteWriteAsync(con => con.UpdateAll(mo, whereExp, m_TableName));


        /// <summary>
        /// 部分字段的更新
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="updateExp">更新字段new{m.Name,....} Or new{m.Name="",....}</param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        public virtual async Task<ResultMo> Update<TType>(TType mo, Expression<Func<TType, object>> updateExp,
            Expression<Func<TType, bool>> whereExp = null)
            => await ExcuteWriteAsync(con => con.UpdatePartail(mo, updateExp, whereExp, m_TableName));

        /// <summary>
        /// 软删除，仅仅修改State状态
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        public virtual async Task<ResultMo> DeleteSoft<TType>( Expression<Func<TType, bool>> whereExp = null, TType mo=null)
            where TType : BaseMo
        {
            mo.status = (int) CommonStatus.Delete;
            return await ExcuteWriteAsync(con => con.UpdatePartail(mo, m => new {m.status }, whereExp, m_TableName));
        }

        /// <summary>
        ///  获取单个实体对象
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        public virtual async Task<ResultMo<TType>> Get<TType>(Expression<Func<TType, bool>> whereExp = null, TType mo = null) where TType : class
            =>await ExcuteReadeAsync(con => con.Get(mo, whereExp, m_TableName));



        /// <summary>
        ///   列表查询
        /// </summary>
        /// <param name="selectSql">查询语句，包含排序等</param>
        /// <param name="totalSql">查询数量语句，不需要排序</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected internal async Task<PageListMo<TType>> GetList<TType>(string selectSql, string totalSql,
            Dictionary<string, object> paras)
            where TType : ResultMo, new()
        {
            return await ExcuteReadeResAsync(async con =>
            {
                var para = new DynamicParameters(paras);
                var total =await con.ExecuteScalarAsync<long>(totalSql, para);
                var list = await con.QueryAsync<TType>(selectSql, para);
                return  new PageListMo<TType>(total, list.ToList());
            });
        }

        #endregion

    }



}






