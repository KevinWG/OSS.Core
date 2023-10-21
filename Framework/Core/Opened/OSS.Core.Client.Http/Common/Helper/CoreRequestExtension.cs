using System.Text.Json;
using OSS.Common.Resp;
using OSS.Tools.Http;

namespace OSS.Core.Client.Http;

/// <summary>
/// 请求扩展
/// </summary>
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
    public static Task<TRes> PostAsync<TRes>(this BaseRemoteRequest req, object? reqBody = null)
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
        var response = await req.SendAsync(req.target_module);
        var strRes   = await response.ReadContentAsStringAsync();

        if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(strRes)){
            throw new RespNetworkException(
                $"接口请求失败，模块{req.target_module}，地址：{req.address_url},消息：{response.ReasonPhrase}\r\n  详情： {strRes}");
        }

        var result = JsonSerializer.Deserialize<TRes>(strRes);
        return result ?? throw new RespNetworkException($"接口请求失败，模块{req.target_module}，地址：{req.address_url}, 响应结果未能转化到类型 {typeof(TRes)}， 响应内容:{strRes}");
    }
}