using OSS.Common.Encrypt;
using OSS.Core.Context;

namespace OSS.Common.Resp;

/// <summary>
/// 用户数据通信令牌辅助类
/// </summary>
public static class PassTokenHelper
{
    /// <summary>
    ///  生成用户数据通行令牌（PassToken）
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static string GenerateToken(string data)
    {
        return GenerateToken(data, string.Empty);
    }

    /// <summary>
    ///  获取用户数据通行令牌中的数据部分
    /// </summary>
    /// <param name="passToken"></param>
    /// <returns></returns>
    public static string GetData(string passToken)
    {
        if (passToken.Length <= 18)
            throw new RespNotAllowedException("无效请求数据!");

        var data   = passToken[18..];
        var random = passToken[..2];

        if (passToken == GenerateToken(data, random))
            return data;

        throw new RespNotAllowedException("无效请求数据!");
    }

    private static string GenerateToken(string data, string randomSeed)
    {
        if (string.IsNullOrEmpty(randomSeed))
            randomSeed = NumHelper.RandomNum(2);
        
        var token = Md5.HalfEncryptHexString(string.Concat(data, CoreContext.App.Identity.token, randomSeed));
        return string.Concat(randomSeed, token, data);
    }




}
