using System;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Tools.Cache;

namespace OSS.Core.Infrastructure
{
    /// <summary>
    ///  返回结果(继承Resp)的对象缓存扩展类 
    /// </summary>
    public static class RespCacheExtension
    {
        /// <summary>
        ///  根据结果清理缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="res"></param>
        /// <param name="cacheKey"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static async Task<TRes> WithCacheClear<TRes>(this Task<TRes> res, string cacheKey, string sourceName = "default")
            where TRes : Resp, new()
        {
            var r = await res;
            if (r.IsSuccess())
            {
                await CacheHelper.RemoveAsync(cacheKey,sourceName);
            }
            return r;
        }

        /// <summary>
        ///  根据结果清理缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="res"></param>
        /// <param name="cacheKey"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static async Task<TRes> WithCacheClear<TRes>(this Task<TRes> res, string[] cacheKey, string sourceName = "default")
            where TRes : Resp, new()
        {
            var r = await res;
            if (r.IsSuccess())
            {
                await CacheHelper.RemoveAsync(cacheKey, sourceName);
            }
            return r;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="getFunc"></param>
        /// <param name="cacheKey"></param>
        /// <param name="slidingTime"></param>
        /// <param name="cacheProtectSeconds"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Task<TRes> WithCache<TRes>(this Func<Task<TRes>> getFunc, string cacheKey, TimeSpan slidingTime,
            int cacheProtectSeconds = 10, string sourceName = "default")
            where TRes : Resp, new()
        {
          return  RespCacheHelper.GetOrSetAsync(cacheKey, getFunc, slidingTime,null, cacheProtectSeconds, sourceName);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="getFunc"></param>
        /// <param name="cacheKey"></param>
        /// <param name="maxAbsoluteTime"></param>
        /// <param name="cacheProtectSeconds"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Task<TRes> WithAbsoluteCache<TRes>(this Func<Task<TRes>> getFunc, string cacheKey,
            TimeSpan maxAbsoluteTime, int cacheProtectSeconds = 10, string sourceName = "default")
            where TRes : Resp, new()
        {
            return RespCacheHelper.GetOrSetAsync(cacheKey, getFunc, null, maxAbsoluteTime, cacheProtectSeconds,
                sourceName);
        }


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="getFunc"></param>
        /// <param name="cacheKey"></param>
        /// <param name="slidingTime"></param>
        /// <param name="maxAbsoluteTime"></param>
        /// <param name="cacheProtectSeconds"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Task<TRes> WithCache<TRes>(this Func<Task<TRes>> getFunc, string cacheKey, TimeSpan? slidingTime,
            TimeSpan? maxAbsoluteTime, int cacheProtectSeconds = 10, string sourceName = "default")
            where TRes : Resp, new()
        {
            return RespCacheHelper.GetOrSetAsync(cacheKey, getFunc, slidingTime, maxAbsoluteTime, cacheProtectSeconds,
                sourceName);
        }
        

    }
}
