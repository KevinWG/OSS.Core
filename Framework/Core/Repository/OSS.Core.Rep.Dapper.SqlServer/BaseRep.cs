#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore仓储层 —— 仓储基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion

using Dapper;

using OSS.Common.Resp;
using OSS.Core.Domain;

using System.Data;
using System.Linq.Expressions;
using OSS.Core.Context;

namespace OSS.Core.Rep.Dapper.SqlServer;

/// <summary>
/// 仓储基类
/// </summary>
/// <typeparam name="TType"></typeparam>
/// <typeparam name="IdType"></typeparam>
public abstract class BaseRep<TType, IdType> : IRepository<TType, IdType>
    //where TType :    IDomainId<IdType>
{
    /// <summary>
    ///  仓储表名
    /// </summary>
    public string TableName { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tableName"></param>
    protected BaseRep(string tableName)
    {
        TableName = tableName;
    }
    
    /// <summary>
    ///  是否存在状态字段
    /// </summary>
    protected static readonly bool HaveStatusColumn;

    /// <summary>
    ///  是否存在状态字段
    /// </summary>
    protected static readonly bool HaveIdColumn;


    /// <summary>
    ///  是否存在状态字段
    /// </summary>
    protected static readonly bool HaveTenantIdColumn;

    static BaseRep()
    {
        var interfaces = typeof(TType).GetInterfaces();
        foreach (var i in interfaces)
        {
            if (i.IsGenericType)
            {
                if (i.GetGenericTypeDefinition() == typeof(IDomainStatus<>))
                    HaveStatusColumn = true;
                else if (i.GetGenericTypeDefinition() == typeof(IDomainId<>))
                    HaveIdColumn = true;
                else if (i.GetGenericTypeDefinition() == typeof(IDomainTenantId<>))
                  HaveTenantIdColumn = true;
            }
        }
    }
    
    /// <summary>
    /// 获取数据库连接
    /// </summary>
    /// <returns></returns>
    protected abstract IDbConnection GetDbConnection(bool isWriteOperate);

    #region Add

    /// <summary>
    ///   插入数据
    /// </summary>
    /// <param name="mo"></param>
    /// <returns></returns>
    public virtual Task Add(TType mo)
    {
        return ExecuteWriteAsync(async con =>
        {
            var row = await con.Insert(TableName, mo);

            if (row <= 0)
                throw new Exception($" ({TableName}) 添加操作失败!");

            return Resp.Success();
        });
    }

    /// <summary>
    ///   插入数据
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public virtual async Task<Resp> AddList(IList<TType> list)
    {
        var res = await ExecuteWriteAsync(async con =>
        {
            var row = await con.InsertList(TableName, list);
            return row > 0 ? new Resp() : new Resp().WithResp(RespCodes.OperateFailed, "添加失败!");
        });
        return res;
    }


    #endregion

    #region Update

    /// <summary>
    /// 部分字段的更新
    ///     参考用法: Update(u => new {u.status}, u => u.id == 1111 ,new{ status });
    /// </summary>
    ///  <param name="updateExp">
    /// 更新字段,示例：
    ///  u=>new{ u.Name, ....},这样生成的参数是同名参数，会从mo对象同名属性中取值
    ///  或者 u=> new{ Name="",mo.Status,....}，这样生成的是匿名参数，参数值即对象本身的值。
    ///  注解：表达式在解析过程中并无实际入参，所以表达式中（TType）u 下的属性仅做类型推断，无实际值，需要通过mo参数传入，where表达式处理相同。 
    /// </param>
    /// <param name="whereExp">
    /// 判断条件 示例：
    ///     w=>w.id==1  , 如果为空默认根据Id判断
    /// </param>
    /// <param name="mo">update和where表达式中参数值</param>
    /// <returns></returns>
    protected virtual Task<Resp> Update(Expression<Func<TType, object>> updateExp,
                                        Expression<Func<TType, bool>> whereExp, object? mo = null)
        => ExecuteWriteAsync(con => con.UpdatePartial(TableName, updateExp, whereExp, mo));

    /// <summary>
    ///  直接使用语句更新操作
    /// </summary>
    /// <param name="updateColNamesSql"></param>
    /// <param name="whereSql"></param>
    /// <param name="para"></param>
    /// <returns></returns>
    protected virtual Task<Resp> Update(string updateColNamesSql, string whereSql, object? para = null)
        => ExecuteWriteAsync(async con =>
        {
            if (string.IsNullOrEmpty(whereSql))
                throw new RespException(SysRespCodes.AppError,"更新语句 where 条件不能为空！");

            var sql = string.Concat("UPDATE ", TableName, " t SET ", updateColNamesSql, " ", whereSql);
            var row = await con.ExecuteAsync(sql, para);

            return row > 0 ? new Resp() : new Resp().WithResp(RespCodes.OperateFailed, "更新失败!");
        });

    #endregion

    #region Delete

    /// <summary>
    /// 软删除，仅仅修改  status = CommonStatus.Delete 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual Task<Resp> SoftDeleteById(IdType id)
    {
        if (!HaveIdColumn)
            throw new NotImplementedException($"当前仓储类型({typeof(TType).Name})未继承(IDomainId<>)，不能使用SoftDeleteById方法！");

        if (id == null)
            throw new Exception(" 删除 Id 不能为空 ！");

        if (!HaveTenantIdColumn)
            return SoftDelete("t.id=@id", new { id });

        const string whereSql = "t.id=@id and t.tenant_id=@tenant_id";
        var tenantId = CoreContext.GetTenantLongId();

        return SoftDelete(whereSql, new { id, tenant_id = tenantId });

    }

    /// <summary>
    /// 软删除，直接修改  status = CommonStatus.Delete 
    /// </summary>
    /// <param name="whereExp">条件表达式</param>
    /// <returns></returns>
    protected virtual Task<Resp> SoftDelete(Expression<Func<TType, bool>> whereExp)
    {
        if (!HaveStatusColumn)
            throw new NotImplementedException($"当前仓储类型({typeof(TType).Name})未继承(IDomainStatus<>)，不能使用SoftDelete方法！");
        
        return Update(m => new {status = CommonStatus.Deleted}, whereExp);
    }

    /// <summary>
    /// 软删除，直接修改状态
    /// </summary>
    /// <param name="whereSql"></param>
    /// <param name="whereParas"></param>
    /// <returns></returns>
    protected virtual Task<Resp> SoftDelete(string whereSql, object? whereParas = null)
    {
        if (!HaveStatusColumn)
            throw new NotImplementedException($"当前仓储类型({typeof(TType).Name})未继承(IDomainStatus<>)，不能使用SoftDelete方法！");
        
        if (string.IsNullOrEmpty(whereSql))
            throw new Exception("更新语句 where 条件不能为空！");
        
        return ExecuteWriteAsync(async con =>
        {
            var sql = $"UPDATE {TableName} t SET t.status={(int) CommonStatus.Deleted} WHERE {whereSql}";

            var rows = await con.ExecuteAsync(sql, whereParas);
            return rows > 0
                ? new Resp()
                : new Resp().WithResp(RespCodes.OperateFailed, "软删除失败!");
        });
    }
    
    #endregion

    #region Get


    /// <summary>
    /// 通过id获取实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual Task<TType> GetById(IdType id)
    {
        return GetById<TType>(id);
    }

    /// <summary>
    /// 通过id获取实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual Task<RType> GetById<RType>(IdType id)
    {
        if (!HaveIdColumn)
            throw new RespNotImplementException($"当前仓储类型({typeof(TType).Name})未继承(IDomainId<>)，不能使用GetById方法！");

        if (id == null)
            throw new Exception(" 查询Id不能为空 ！");

        var sql= string.Concat("select * from ", TableName, " t WHERE t.id=@id");
        var dirPara = new Dictionary<string, object> {{"@id", id}};

        if (HaveStatusColumn)
        {
            sql = string.Concat(sql, " and t.status>@status");
            dirPara.Add("@status", (int) CommonStatus.Deleted);
        }

        if (HaveTenantIdColumn)
        {
            sql = string.Concat(sql, " and t.tenant_id=@tenant_id");
            dirPara.Add("@tenant_id", CoreContext.GetTenantLongId());
        }

        return Get<RType>(sql, dirPara);
    }

    /// <summary>
    ///  获取单个实体对象
    /// </summary>
    /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
    /// <returns></returns>
    protected Task<TType> Get(Expression<Func<TType, bool>> whereExp)
        => ExecuteReadAsync(con => con.Get<TType, TType>(TableName, whereExp));


    /// <summary>
    ///  获取单个实体对象
    /// </summary>
    /// <param name="whereExp">判断条件，如果为空默认根据Id判断</param>
    /// <returns></returns>
    protected Task<RType> Get<RType>(Expression<Func<TType, bool>> whereExp)
        => ExecuteReadAsync(con => con.Get<TType, RType>(TableName, whereExp));

    /// <summary>
    /// 通过sql语句获取实体
    /// </summary>
    /// <param name="getSql"> 查询sql语句</param>
    /// <param name="para"></param>
    /// <returns></returns>
    protected virtual Task<RType> Get<RType>(string getSql, object para)
    {
        return ExecuteReadAsync(con => con.QuerySingleOrDefaultAsync<RType>(getSql, para));
    }

    #endregion

    #region Get（Page）List

    /// <summary>
    ///   列表查询
    /// </summary>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    protected virtual Task<List<TType>> GetList(Expression<Func<TType, bool>> whereExp)
        => ExecuteReadAsync(con => con.GetList<TType, TType>(TableName, whereExp));

    /// <summary>
    ///   列表查询
    /// </summary>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    protected virtual Task<List<RType>> GetList<RType>(Expression<Func<TType, bool>> whereExp)
        => ExecuteReadAsync(con => con.GetList<TType, RType>(TableName, whereExp));

    /// <summary>
    ///   列表查询
    /// </summary>
    /// <param name="getSql">查询语句</param>
    /// <param name="paras">参数内容</param>
    /// <returns></returns>
    protected Task<List<TType>> GetList(string getSql, object? paras)
    {
        return GetList<TType>(getSql, paras);
    }

    /// <summary>
    ///   列表查询
    /// </summary>
    /// <param name="getSql">查询语句</param>
    /// <param name="paras">参数内容</param>
    /// <returns></returns>
    protected virtual Task<List<RType>> GetList<RType>(string getSql, object? paras)
    {
        return ExecuteReadAsync(async con =>
        {
            var list = await con.QueryAsync<RType>(getSql, paras);
            return list.ToList();
        });
    }
    
    /// <summary>
    ///   列表查询
    /// </summary>
    /// <param name="selectSql">查询语句，包含排序等</param>
    /// <param name="totalSql">查询数量语句，不需要排序,如果为空，则不计算和返回总数信息</param>
    /// <param name="paras">参数内容</param>
    /// <returns></returns>
    protected Task<PageList<TType>> GetPageList(string selectSql, object? paras, string? totalSql = null)
    {
        return GetPageList<TType>(selectSql, paras, totalSql);
    }

    /// <summary>
    ///   列表查询
    /// </summary>
    /// <param name="selectSql">查询语句，包含排序等</param>
    /// <param name="totalSql">查询数量语句，不需要排序,如果为空，则不计算和返回总数信息</param>
    /// <param name="paras">参数内容</param>
    /// <returns></returns>
    protected virtual async Task<PageList<RType>> GetPageList<RType>(string selectSql, object? paras,
                                                                     string? totalSql = null)
    {
        return await ExecuteReadAsync(async con =>
        {
            var total = 0;
            if (!string.IsNullOrEmpty(totalSql))
            {
                total = await con.ExecuteScalarAsync<int>(totalSql, paras);
                if (total <= 0) return new PageList<RType>(0, new List<RType>());
            }

            var list = (await con.QueryAsync<RType>(selectSql, paras)).ToList();
            return new PageList<RType>(total, list);
        });
    }

    #endregion

    #region 底层基础读写分离操作封装

    /// <summary>
    /// 执行写数据库操作
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="paras"></param>
    /// <returns></returns>
    protected Task<Resp> ExecuteWriteAsync(string sql, object paras)
    {
        return ExecuteWriteAsync(async con =>
        {
            var rows = await con.ExecuteAsync(sql, paras);
            return (rows > 0 ? new Resp() : new Resp(RespCodes.OperateFailed, "未能执行成功！"));
        });
    }

    /// <summary>
    /// 执行写数据库操作
    /// </summary>
    /// <typeparam name="RespType"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    protected Task<RespType> ExecuteWriteAsync<RespType>(Func<IDbConnection, Task<RespType>> func)
        => ExecuteAsync(func, true);



    /// <summary>
    ///  执行读操作，返回具体类型，自动包装成Resp结果实体
    /// </summary>
    /// <typeparam name="RespParaType"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    protected Task<Resp<RespParaType>> ExecuteReadAsRespAsync<RespParaType>(
        Func<IDbConnection, Task<RespParaType>> func)
        => ExecuteAsync(async con =>
        {
            var res = await func(con);

            return (res != null
                ? new Resp<RespParaType>(res)
                : new Resp<RespParaType>().WithResp(RespCodes.OperateObjectNull, "未发现相关数据！"));
        }, false);
    


    /// <summary>
    /// 执行读操作，直接返回继承自Resp实体
    /// </summary>
    /// <typeparam name="SubRespType"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    protected async Task<SubRespType> ExecuteReadAsync<SubRespType>(Func<IDbConnection, Task<SubRespType>> func)
        => await ExecuteAsync(func, false);

    /// <summary>
    ///  最终执行操作
    /// </summary>
    /// <typeparam name="RType"></typeparam>
    /// <param name="func"></param>
    /// <param name="isWrite"></param>
    /// <returns></returns>
    private async Task<RType> ExecuteAsync<RType>(Func<IDbConnection, Task<RType>> func, bool isWrite)
    {
        //RType t;
        //try
        //{
        using var con = GetDbConnection(isWrite);
        return await func(con);
        //}
        //catch (Exception e)
        //{
        //    LogHelper.Error(string.Concat("数据库操作错误,仓储表名：", TableName, "，详情：", e.Message, "\r\n", e.StackTrace),
        //        "DataRepConnectionError",
        //        "DapperRep_Mysql");
        //    t = new RType
        //    {
        //        sys_ret =  (int) SysRespCodes.ApplicationError,
        //        ret = (int) RespCodes.InnerError,
        //        msg = isWrite ? "数据操作出错！" : "数据读取错误"
        //    };
        //}

        //return t;
    }

    #endregion
}







