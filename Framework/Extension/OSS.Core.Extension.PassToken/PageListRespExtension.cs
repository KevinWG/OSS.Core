using OSS.Core.Domain;

namespace OSS.Common.Resp;

/// <summary>
/// 
/// </summary>
public static class PageListRespExtension
{

    /// <summary>
    /// 转化通行token分页列表
    /// -附带指定列的token处理
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="pageList"></param>
    /// <param name="tokenColumnName">关联的key列名称</param>
    /// <param name="tokenKeySelector">对应 tokenKeyColumnName 列的 token key 选择器</param>
    /// <returns></returns>
    public static TokenPageListResp<TData> ToTokenPageResp<TData>(this PageListResp<TData> pageList,
                                                                  string tokenColumnName,
                                                                  Func<TData, string> tokenKeySelector)
    {
        return pageList.ToTokenPageResp().AddColumnToken(tokenColumnName, tokenKeySelector);
    }



    private static string GetId(BaseMo<long> mo) => mo.id.ToString();


    /// <summary>
    /// 转化为通行token分页列表
    /// 并附带默认以Id为列的token处理
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="pageList"></param>
    /// <returns></returns>
    public static TokenPageListResp<TData> ToTokenPageRespWithIdToken<TData>(this PageListResp<TData> pageList)
        where TData : BaseMo<long>
    {
        return ToTokenPageResp(pageList, "id", GetId);
    }

    /// <summary>
    /// 转化为通行token分页列表
    /// 并附带默认以Id为列的token处理
    /// </summary>
    /// <returns></returns>
    public static async Task<TokenPageListResp<TData>> ToTokenPageRespWithIdToken<TData>(
        this Task<PageListResp<TData>> taskPageList)
        where TData : BaseMo<long>
    {
        var pageList = await taskPageList;
        return ToTokenPageRespWithIdToken(pageList);
    }


    /// <summary>
    /// 转化为通行token分页列表
    /// 并附带默认以Id为列的token处理
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="pageList"></param>
    /// <returns></returns>
    public static TokenPageListResp<TData> ToTokenPageRespWithStrIdToken<TData>(this PageListResp<TData> pageList)
        where TData : BaseMo<string>
    {
        return ToTokenPageResp(pageList, "id", d=>d.id);
    }

}
