namespace OSS.Common.Resp;

/// <summary>
/// 
/// </summary>
public static class TokenPageListRespExtension
{
    /// <summary>添加列表通行token</summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="pageList"></param>
    /// <param name="tokenColumnName">关联的key列名称</param>
    /// <param name="tokenKeySelector">对应 tokenKeyColumnName 列的 token key 选择器</param>
    /// <returns></returns>
    public static TokenPageListResp<TData> AddColumnToken<TData>(this TokenPageListResp<TData> pageList,
                                                                 string tokenColumnName,
                                                                 Func<TData, string> tokenKeySelector)
    {
        pageList.AddColumnToken(tokenColumnName, tokenKeySelector,
            x => PassTokenHelper.GenerateToken(tokenKeySelector(x)));
        return pageList;
    }
}