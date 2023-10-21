using OSS.Core.Domain;

namespace OSS.Common.Resp;

/// <summary>
/// 
/// </summary>
public static class ListTokenRespExtension
{
    /// <summary>添加列表通行token</summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="listRes"></param>
    /// <param name="tokenColumnName">关联的key列名称</param>
    /// <param name="tokenKeySelector">对应 tokenKeyColumnName 列的 token key 选择器</param>
    /// <returns></returns>
    public static TokenListResp<TData> AddColumnToken<TData>(this TokenListResp<TData> listRes,
                                                             string tokenColumnName, Func<TData, string> tokenKeySelector)
    {
        listRes.AddColumnToken(tokenColumnName, tokenKeySelector,
            x => PassTokenHelper.GenerateToken(tokenKeySelector(x)));
        return listRes;
    }

    /// <summary>
    /// 转化通行token分页列表
    /// -附带指定列的token处理
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="listRes"></param>
    /// <param name="tokenColumnName">关联的key列名称</param>
    /// <param name="tokenKeySelector">对应 tokenKeyColumnName 列的 token key 选择器</param>
    /// <returns></returns>
    public static TokenListResp<TData> ToTokenListResp<TData>(this ListResp<TData> listRes,
                                                          string tokenColumnName, Func<TData, string> tokenKeySelector)
    {
        return listRes.ToTokenListResp().AddColumnToken(tokenColumnName, tokenKeySelector);
    }

    /// <summary>
    /// 转化为通行token分页列表
    /// 并附带默认以Id为列的token处理
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="listRes"></param>
    /// <returns></returns>
    public static TokenListResp<TData> ToTokenListRespWithIdToken<TData>(this ListResp<TData> listRes)
        where TData : BaseMo<long>
    {
        return ToTokenListResp(listRes, "id", GetId);
    }

    /// <summary>
    /// 转化为通行token分页列表
    /// 并附带默认以Id为列的token处理
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static TokenListResp<TData> ToTokenListRespWithIdToken<TData>(this IList<TData> list)
        where TData : BaseMo<long>
    {
        return list.ToTokenListResp().AddColumnToken("id", GetId); //ToTokenListResp(listRes, "id", GetId);
    }

    private static string GetId(BaseMo<long> mo) => mo.id.ToString();
}

