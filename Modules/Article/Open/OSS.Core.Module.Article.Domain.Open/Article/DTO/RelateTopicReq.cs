using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///   添加文章专题关联请求
/// </summary>
public class RelateTopicReq
{
    /// <summary>
    ///  文章id
    /// </summary>
    public long article_id { get; set; }

    /// <summary>
    ///  专题
    /// </summary>
    public List<long>? topics { get; set; }
}


/// <summary>
///  文章专题关联转化映射
/// </summary>
public static class RelateTopicReqMap
{
    /// <summary>
    ///  转化为文章专题关联对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static List<ArticleTopicMo> MapToArticleTopicMos(this RelateTopicReq req)
    {
        if (req.topics == null || req.topics.Count == 0)
        {
            throw new RespArgumentException("topics", "投送专题不能为空!");
        }

        var atList = new List<ArticleTopicMo>();

        foreach (var topic in req.topics)
        {
            var mo = new ArticleTopicMo
            {
                article_id = req.article_id,
                topic_id   = topic
            };

            mo.FormatBaseByContext();
            atList.Add(mo);
        }

        return atList;

    }
}
