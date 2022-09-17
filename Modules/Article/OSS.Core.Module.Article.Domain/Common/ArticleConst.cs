﻿namespace OSS.Core.Module.Article;

/// <summary>
///  Article 模块静态声明
/// </summary>
public static class ArticleConst
{
    /// <summary>
    ///  模块名称
    /// </summary>
    public const string ModuleName = "Article";

    public static class CacheKeys
    {
        // 放置相关缓存Key
        // 涉及相关动态参数，建议以 ByPara 结尾，例如： ArticleDetailById = "ArticleDetail_"
    }

    public static class DataMsgKeys
    {
        // 放置 发布订阅/异步 消息key
    }

    public static class FuncCodes
    {
        // 放置权限码
    }

    /// <summary>
    ///  仓储表名
    /// </summary>
    public static class RepTables
    {
        /// <summary>
        ///  文章表
        /// </summary>
        public const string Article = "m_article";

        /// <summary>
        ///  专题表
        /// </summary>
        public const string Topic = "m_article_topic";

        /// <summary>
        /// 文章主题关联表
        /// </summary>
        public const string ArticleTopic = "m_article_topic_relate";
    }
}
