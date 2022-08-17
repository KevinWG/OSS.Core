using System.Text.Json;
using OSS.Tools.Http;

namespace OSS.Core.Client.Http;

public static class CoreRequestExtension
{
    /// <summary>
    ///  Get请求
    /// </summary>
    /// <typeparam name="TRes"></typeparam>
    /// <param name="req"></param>
    /// <returns></returns>
    public static Task<TRes> GetAsync<TRes>(this BaseRemoteRequest req)
    {
        req.http_method = HttpMethod.Get;
        return SendAsync<TRes>(req);
    }

    /// <summary>
    ///  Post 请求
    /// </summary>
    /// <typeparam name="TRes"></typeparam>
    /// <param name="req"></param>
    /// <param name="reqBody"></param>
    /// <returns></returns>
    public static Task<TRes> PostAsync<TRes>(this BaseRemoteRequest req, object? reqBody=null)
    {
        var content = reqBody == null ? string.Empty : JsonSerializer.Serialize(reqBody);

        req.custom_body = content;
        req.http_method = HttpMethod.Post;

        return SendAsync<TRes>(req);
    }
    
    /// <summary>
    ///  发送请求
    /// </summary>
    /// <typeparam name="TRes"></typeparam>
    /// <param name="req"></param>
    /// <returns></returns>
    public static async Task<TRes> SendAsync<TRes>(this BaseRemoteRequest req)
    {
        var strRes = await req.SendAsync(req.target_module).ReadContentAsStringAsync();
        return JsonSerializer.Deserialize<TRes>(strRes);
    }
}