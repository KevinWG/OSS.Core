using System.Text;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipeline 对象仓储
/// </summary>
public class PipelineVersionRep : BasePipelineRep<VersionMo, long>
{
    /// <inheritdoc />
    public PipelineVersionRep() : base(PipelineConst.RepTables.PipelineVersion)
    {
    }


    private const string _pipelineCols = " PV.*,  P.type, P.execute_ext,P.parent_id";
    private const string _orderSql     = " order by PV.id desc ";

    /// <summary>
    ///  查询流水线列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<PipelineMo>> SearchLines(SearchReq req)
    {
        const string lineTables = @$"  {PipelineConst.RepTables.PipelineVersion} PV
inner join {PipelineConst.RepTables.PipelineMeta} PM on PM.latest_pipe_id = PV.id
inner join {PipelineConst.RepTables.Pipe} P on P.id = PV.id ";
        

        var paras    = new Dictionary<string, object>();
        var whereSql = GetSearchLinesWhereSql(req, paras);

        var selectSql = string.Concat("select ", _pipelineCols, " from ", lineTables, whereSql, _orderSql, " limit ",
            req.size, " offset ", req.GetStartRow());
        var selectCountSql =
            req.req_count ? string.Concat("select count(0) from ", lineTables, whereSql) : string.Empty;

        return GetPageList<PipelineMo>(selectSql, paras, selectCountSql);
    }

    private static string GetSearchLinesWhereSql(SearchReq req, Dictionary<string, object> paras)
    {
        var filter = req.filter;
        if (filter == null)
            return string.Empty;

        var whereSql = new StringBuilder();
        whereSql.Append(" where 1=1 ");

        foreach (var filterItem in filter)
        {
            switch (filterItem.Key.ToLower())
            {
                //case "name":

            }
        }

        return whereSql.ToString();
    }

    /// <summary>
    ///  获取流水线所有历史版本
    /// </summary>
    /// <param name="metaId"></param>
    /// <returns></returns>
    public Task<List<PipelineMo>> GetVersions(long metaId)
    {
        const string tableName = @$"  {PipelineConst.RepTables.PipelineVersion} PV
inner join {PipelineConst.RepTables.Pipe} P on P.id = PV.id ";

        var sql = string.Concat("select ", _pipelineCols, " from ", tableName, " where PV.meta_id = @meta_id and PV.status>@status");
        return GetList<PipelineMo>(sql, new { meta_id  = metaId, status =(int)PipelineStatus.Deleted});
    }
    
    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<IResp> UpdateStatus(long id, PipelineStatus status)
    {
        return Update(u => new {u.status}, w => w.id == id, new {status});
    }
}
