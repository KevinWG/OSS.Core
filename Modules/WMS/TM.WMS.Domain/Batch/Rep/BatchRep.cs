using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///  批次号 对象仓储
/// </summary>
public class BatchRep : BaseWMSRep<BatchMo,long> 
{
    /// <inheritdoc />
    public BatchRep() : base(WMSConst.Tables.batch)
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<BatchMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }

    /// <inheritdoc />
    protected override string BuildSimpleSearch_FilterItemSql(string key, string value, Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "code":
                sqlParas.Add("@batch_code",value);
                return " t.`code`=@batch_code";

            case "material_code":
                sqlParas.Add("@material_code", value);
                return " t.`material_code`=@material_code";

            case "expire_start_at":
                var startAt = value.ToInt64();
                if (startAt <= 0)
                    return string.Empty;

                sqlParas.Add("@expire_start_at", startAt);
                return " t.`expire_date`>=@expire_start_at";

            case "expire_end_at":
                var endAt = value.ToInt64();
                if (endAt <= 0)
                    return string.Empty;

                sqlParas.Add("@expire_end_at", endAt);
                return " t.`expire_date`<=@expire_end_at";
        }
        return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
    }

    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<Resp> UpdateStatus(long id, CommonStatus status)
    {
        return Update(u => new {u.status}, w => w.id == id, new {status});
    }


    /// <summary>
    ///  查看code存在数量，判断是否存在
    /// </summary>
    /// <param name="batchCode"></param>
    /// <returns></returns>
    public Task<Resp<int>> GetCountByCode(string batchCode)
    {
        var tenantId = CoreContext.GetTenantLongId();
        var sql      = $"SELECT count(0) FROM {TableName} t where t.code=@code and t.tenant_id=@tenant_id";
  
        return Get<int>(sql, new { tenant_id = tenantId, code = batchCode });
    }
}

///// <summary>
///// 批次号相关搜索参数
///// </summary>
//public class BatchCodeSearchPara
//{
//    /// <summary>
//    ///  物料编码
//    /// </summary>
//    public string material_code { get; set; } = string.Empty;

//    /// <summary>
//    ///  批次号
//    /// </summary>
//    public string batch_code { get; set; } = string.Empty;

//    /// <summary>
//    ///  有效期区间的开始时间
//    /// </summary>
//    public long expire_start_at { get; set; }

//    /// <summary>
//    ///  有效期区间的开始时间
//    /// </summary> 
//    public long expire_end_at { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public int status { get; set; }
//}
