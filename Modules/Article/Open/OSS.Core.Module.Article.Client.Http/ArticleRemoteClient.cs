using OSS.Common;

namespace OSS.Core.Module.Article.Client;

/// <summary>
/// Article 模块接口客户端
/// </summary>
public static class ArticleRemoteClient //: IArticleClient
{
    /// <summary>
    ///  Topic 接口
    /// </summary>
    public static ITopicOpenService Topic {get; } = SingleInstance<TopicHttpClient>.Instance;

    /// <summary>
    ///  Article 接口
    /// </summary>
    public static IArticleOpenService Article {get; } = SingleInstance<ArticleHttpClient>.Instance;

    /// <summary>
    ///  Category 接口
    /// </summary>
    public static ICategoryOpenService Category {get; } = SingleInstance<CategoryHttpClient>.Instance;
}