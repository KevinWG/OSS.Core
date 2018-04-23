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
using OSS.Common.Plugs.LogPlug;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Infrastructure.Mos;
using OSS.Core.Infrastructure.Utils;
using OSS.Plugs.OrmMysql.OrmExtention;

namespace OSS.Plugs.OrmMysql
{
    /// <summary>
    /// 仓储层基类
    /// </summary>
    public class BaseMysqlRep<TRep,TType>
        where TRep:class ,new()
        where TType:BaseMo,new()
    {
        protected static string m_TableName;
        
        protected static string m_writeConnectionString;
        protected static string m_readeConnectionString;

        static BaseMysqlRep()
        {
            m_writeConnectionString = ConfigUtil.GetConnectionString("WriteConnection");
            m_readeConnectionString = ConfigUtil.GetConnectionString("ReadeConnection");
            SqlVistorFlag.SetDbProvider(SqlVistorProvider.Mysql);
        }

        #region 底层基础读写分离封装

        /// <summary>
        /// 执行写数据库操作
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal static async Task<RType> ExcuteWriteAsync<RType>(Func<IDbConnection, Task<RType>> func) where RType : ResultMo, new()
            =>await Execute(func, m_writeConnectionString);

        /// <summary>
        ///  执行读操作，返回具体类型，自动包装成ResultMo结果实体
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal static async Task<ResultMo<RType>> ExcuteReadeAsync<RType>(Func<IDbConnection, Task<RType>> func) =>await Execute(async con =>
        {
            var res =await func(con);
            return res != null ? new ResultMo<RType>(res) : new ResultMo<RType>(ResultTypes.ObjectNull, "未发现相关数据！");
        }, m_readeConnectionString);

        /// <summary>
        /// 执行读操作，直接返回继承自ResultMo实体
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal static async Task<RType> ExcuteReadeResAsync<RType>(Func<IDbConnection, Task<RType>> func) where RType : ResultMo, new()
            =>await Execute(func, m_readeConnectionString);

        private static async Task<RType> Execute<RType>(Func<IDbConnection, Task<RType>> func, string connecStr)
            where RType : ResultMo, new()
        {
            RType t;

            try
            {
                using (var con = new MySqlConnection(connecStr))
                {
                    t = await func(con);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw e;
#endif
                LogUtil.Error(string.Concat("数据库操作错误，详情：", e.Message, "\r\n", e.StackTrace), "DataRepConnectionError",
                    "DapperRep");
                t = new RType
                {
                    ret = (int) ResultTypes.InnerError,
                    msg = "数据更新出错！"
                };
            }

            return t ?? new RType() {ret = (int) ResultTypes.ObjectNull, msg = "未发现对应结果"};
        }

        #endregion

        #region   基础CRUD操作方法

        /// <summary>
        ///   插入数据
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        public virtual async Task<ResultIdMo> Add<MType>(MType mo)
        where MType:BaseMo
            =>await ExcuteWriteAsync(con => con.Insert(m_TableName,mo));
        
        /// <summary>
        /// 部分字段的更新
        /// </summary>
        ///  <param name="updateExp">更新字段new{m.Name,....} Or new{ Name="",....}</param>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <param name="mo"></param>
        /// <returns></returns>
        protected virtual async Task<ResultMo> Update(Expression<Func<TType, object>> updateExp,
            Expression<Func<TType, bool>> whereExp, object mo = null)
            => await ExcuteWriteAsync(con => con.UpdatePartail(m_TableName, updateExp, whereExp, mo));
        
        protected virtual async Task<ResultMo> Update(string updateSql,string whereSql, object para = null)
            => await ExcuteWriteAsync(async con =>
            {
                var sql = string.Concat("UPDATE ", m_TableName, " SET ", updateSql, whereSql);
                var row = await con.ExecuteAsync(sql, para);
                return row > 0 ? new ResultMo() : new ResultMo(ResultTypes.UpdateFail, "更新失败");
            });


        /// <summary>
        ///  获取单个实体对象
        /// </summary>
        /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
        /// <returns></returns>
        protected virtual async Task<ResultMo<TType>> Get(Expression<Func<TType, bool>> whereExp)
            =>await ExcuteReadeAsync(con => con.Get(m_TableName, whereExp));

        /// <summary>
        ///   列表查询
        /// </summary>
        /// <param name="selectSql">查询语句，包含排序等</param>
        /// <param name="totalSql">查询数量语句，不需要排序</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected internal static async Task<PageListMo<TType>> GetPageList(string selectSql, string totalSql,
            object paras)
        {
            return await ExcuteReadeResAsync(async con =>
            {
                var total = await con.ExecuteScalarAsync<long>(totalSql, paras);
                if (total <= 0) return new PageListMo<TType>(ResultTypes.ObjectNull, "没有查到相关信息！");

                var list = await con.QueryAsync<TType>(selectSql, paras);
                return new PageListMo<TType>(total, list.ToList());
            });
        }

        protected const string DefaultOrderSql = " ORDER BY `id` DESC";
        
        /// <summary>
        ///   列表查询
        /// </summary>
        /// <param name="whereExp"></param>
        /// <returns></returns>
        protected internal static async Task<ResultMo<IList<TType>>> GetList(Expression<Func<TType, bool>> whereExp)
            => await ExcuteReadeAsync(con => con.GetList(m_TableName, whereExp));


    
        protected static async Task<ResultMo<TType>> GetById(long id)
        {
            var dirPara = new Dictionary<string, object> {{"@id", id}};
            var sql = string.Concat("select * from ", m_TableName, " WHERE id=@id");
            return await Get(sql, dirPara);
        }

        /// <summary>
        /// 通过sql语句获取实体
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        protected internal static async Task<ResultMo<TType>> Get(string sql, object para)
        {
            return await ExcuteReadeAsync(con => con.QuerySingleOrDefaultAsync<TType>(sql, para));
        }

        /// <summary>
        /// 软删除，仅仅修改State状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ResultMo> SoftDeleteById(long id)
        {
            var sql = string.Concat("UPDATE ", m_TableName, " SET status=@status WHERE id=@id");
            var dirPara = new Dictionary<string, object> {{"@id", id}, {"@status", (int) CommonStatus.Delete}};

            var deRes = await ExcuteWriteAsync(async con =>
            {
                var rows = await con.ExecuteAsync(sql, dirPara);
                return rows > 0 ? new ResultMo() : new ResultMo(ResultTypes.UpdateFail, "soft delete Failed!");
            });
            return deRes;
        }

        /// <summary>
        /// 强制删除，删除物理数据【慎用】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ResultMo> ForceDeleteById(long id)
        {
            var sql = string.Concat("delete from ", m_TableName, " WHERE `id`=@id");
            var dirPara = new Dictionary<string, object> {{"@id", id}};

            var deRes = await ExcuteWriteAsync(async con =>
            {
                var rows = await con.ExecuteAsync(sql, dirPara);
                return rows > 0 ? new ResultMo() : new ResultMo(ResultTypes.UpdateFail, "force delete Failed");
            });
            return deRes;
        }

        #endregion

        #region 单例模块

        private static object _lockObj = new object();

        private static TRep _instance;

        /// <summary>
        ///   接口请求实例  
        ///  当 DefaultConfig 设值之后，可以直接通过当前对象调用
        /// </summary>
        public static TRep Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (_lockObj)
                {
                    if (_instance == null)
                        _instance = new TRep();
                }
                return _instance;
            }

        }

        #endregion

    }



}






