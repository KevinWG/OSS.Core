using OSS.Common.Resp;

namespace OSS.Tools.Cache;

/// <summary>
///  返回结果(继承IResp)的对象缓存扩展类 
/// </summary>
public static class RespCacheExtension
{
    /// <summary>
    ///  根据返回结果判断清理缓存
    /// </summary>
    /// <typeparam name="TRes"></typeparam>
    /// <param name="res"></param>
    /// <param name="cacheKey"></param>
    /// <param name="sourceName"></param>
    /// <returns></returns>
    public static async Task<TRes> WithRespCacheClearAsync<TRes>(this Task<TRes> res, string cacheKey, string sourceName = "default")
        where TRes : IResp
    {
        var r = await res;
        if (r.IsSuccess())
        {
            await CacheHelper.RemoveAsync(cacheKey, sourceName);
        }
        return r;
    }

    /// <summary>
    ///  根据返回结果判断清理缓存
    /// </summary>
    /// <typeparam name="TRes"></typeparam>
    /// <param name="res"></param>
    /// <param name="cacheKey"></param>
    /// <param name="sourceName"></param>
    /// <returns></returns>
    public static async Task<TRes> WithRespCacheClearAsync<TRes>(this Task<TRes> res, string[] cacheKey,
                                                                 string sourceName = "default")
        where TRes : IResp
    {
        var r = await res;
        if (r.IsSuccess())
        {
            await CacheHelper.RemoveAsync(cacheKey, sourceName);
        }

        return r;
    }


    /// <summary>
    /// 获取缓存数据，如果结果成功则添加，否则放弃或缓存保护
    /// </summary>
    /// <typeparam name="RType"></typeparam>
    /// <param name="cacheKey">key</param>
    /// <param name="getFunc">获取原始数据方法</param>
    /// <param name="slidingExpiration">滚动过期时长，访问后自动延长</param>
    /// <param name="hitProtectedSeconds">缓存击穿保护秒数，默认值10。</param>
    /// <param name="sourceName">来源名称</param>
    /// <returns></returns>
    public static Task<RType> WithRespCacheAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey,
                                                    TimeSpan slidingExpiration, int hitProtectedSeconds = 10,
                                                    string sourceName = "default")
        where RType : IResp
    {
        return CacheHelper.GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions()
        {
            sliding_expiration = slidingExpiration
        }, res => res.IsSuccess(), hitProtectedSeconds, sourceName);
    }


    /// <summary>
    /// 获取缓存数据【同时添加缓存击穿保护】，如果结果成功则添加，否则放弃或缓存保护
    /// </summary>
    /// <typeparam name="RType"></typeparam>
    /// <param name="cacheKey">key</param>
    /// <param name="getFunc">获取原始数据方法</param>
    /// <param name="absoluteExpiration">固定过期时长，设置后到时过期</param>
    /// <param name="hitProtectedSeconds">缓存击穿保护秒数，默认值10。</param>
    /// <param name="sourceName">来源名称</param>
    /// <returns></returns>
    public static Task<RType> WithRespAbsoluteCacheAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey,
                                                            TimeSpan absoluteExpiration,
                                                            int hitProtectedSeconds = 10, string sourceName = "default")
        where RType : IResp
    {
        return CacheHelper.GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions()
        {
            absolute_expiration_relative_to_now = absoluteExpiration
        }, res => res.IsSuccess(), hitProtectedSeconds, sourceName);
    }



    /// <summary>
    /// 获取缓存数据，如果结果成功则添加，否则放弃或缓存保护
    /// </summary>
    /// <typeparam name="RType">数据类型</typeparam>
    /// <param name="cacheKey"></param>
    /// <param name="getFunc">获取原始数据方法</param>
    /// <param name="cacheTimeOpt">缓存时间信息</param>
    /// <param name="hitProtectedSeconds">缓存击穿保护秒数，默认值10。</param>
    /// <param name="sourceName">来源名称</param>
    /// <returns></returns>
    public static Task<RType> WithRespCacheAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey,
                                                    CacheTimeOptions cacheTimeOpt,
                                                    int hitProtectedSeconds = 10, string sourceName = "default")
        where RType : IResp
    {
        return CacheHelper.GetOrSetAsync(cacheKey, getFunc, cacheTimeOpt, res => !res.IsSuccess(), hitProtectedSeconds,
            sourceName);
    }
    
}