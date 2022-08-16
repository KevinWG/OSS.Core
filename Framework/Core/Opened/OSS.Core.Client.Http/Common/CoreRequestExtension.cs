using System.Text.Json;
using OSS.Tools.Http;

namespace OSS.Core.Client.Http;

public static class CoreRequestExtension
{
    public static async Task<TRes> GetAsync<TRes>(this CoreRequest req)
    {
        var strRes = await req.GetAsync().ReadContentAsStringAsync();
        return JsonSerializer.Deserialize<TRes>(strRes);
    }

    public static async Task<TRes> PostAsync<TRes>(this CoreRequest req, object reqBody)
    {
        var strRes = await req.PostAsync(JsonSerializer.Serialize(reqBody)).ReadContentAsStringAsync();
        return JsonSerializer.Deserialize<TRes>(strRes);
    }
}