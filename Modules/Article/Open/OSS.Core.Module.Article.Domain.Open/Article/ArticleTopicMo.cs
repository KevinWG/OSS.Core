
#region Copyright (C)  Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 实体对象
*
*　　	创建人： osscore
*    	创建日期：
*       
*****************************************************************************/

#endregion

using OSS.Core.Domain;

namespace OSS.Core.Module.Article;

/// <summary>
///  文章专题关联 对象实体 
/// </summary>
public class ArticleTopicMo : BaseOwnerAndStateMo<long>
{
    /// <summary>
    ///  文章Id
    /// </summary>
    public long article_id { get; set; }

    /// <summary>
    ///  专题Id
    /// </summary>
    public long topic_id { get; set; }
}
