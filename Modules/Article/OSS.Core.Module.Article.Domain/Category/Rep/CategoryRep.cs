using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  Category 对象仓储
/// </summary>
public class CategoryRep : BaseArticleRep<CategoryMo,long> 
{


    /// <inheritdoc />
    public CategoryRep() : base("m_article_category")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<CategoryMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }

    /// <inheritdoc />
    protected override string BuildSimpleSearch_FilterItemSql(string key, string value, Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "parent_id":
                sqlParas.Add("@parent_id",value.ToInt64());
                return " parent_id=@parent_id ";
        }
        return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
    }

    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<IResp> UpdateStatus(long id, CommonStatus status)
    {
        return Update(u => new {u.status}, w => w.id == id, new {status});
    }

    /// <summary>
    ///  获取当前节点下的最后一个子节点
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public Task<IResp<long>> GetLastSubId(long parentId)
    {
        // 包含已经被软删除的数据
         var sql = $"SELECT id FROM { TableName } t where t.parent_id=@parent_id order by id desc limit 1";

        return Get<long>(sql, new { parent_id = parentId });
    }
}
