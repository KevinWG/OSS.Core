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
    public static async Task<TRes> GetAsync<TRes>(this BaseCoreRequest req)
    {
        var strRes = await req.GetAsync().ReadContentAsStringAsync();
        return JsonSerializer.Deserialize<TRes>(strRes);
    }

    /// <summary>
    ///  Post 请求
    /// </summary>
    /// <typeparam name="TRes"></typeparam>
    /// <param name="req"></param>
    /// <param name="reqBody"></param>
    /// <returns></returns>
    public static async Task<TRes> PostAsync<TRes>(this BaseCoreRequest req, object reqBody=null)
    {
        var content = reqBody == null ? string.Empty : JsonSerializer.Serialize(reqBody);

        var strRes  = await req.PostAsync(content).ReadContentAsStringAsync();
        return JsonSerializer.Deserialize<TRes>(strRes);
    }
}