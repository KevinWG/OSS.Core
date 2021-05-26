using System;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Tools.Cache;

namespace OSS.Core.Infrastructure.Extensions
{
    public static class CacheExtension
    {
        /// <summary>
        ///  根据结果清理缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="res"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static async Task<TRes> WithCacheClear<TRes>(this Task<TRes> res,params string[] cacheKey)
            where TRes : Resp
        {
            var r = await res;
            if (r.IsSuccess())
            {
                await CacheHelper.RemoveAsync(cacheKey);
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
        /// <returns></returns>
        public static Task<TRes> WithCache<TRes>(this Func<Task<TRes>> getFunc, string cacheKey,TimeSpan slidingTime,int cacheProtectSeconds = 10, string sourceName = "default")
            where TRes : Resp
        {
            return cacheProtectSeconds==0 
                ?  CacheHelper.GetOrSetAsync(cacheKey, getFunc, slidingTime, null, sourceName)
                : WithCache(getFunc, cacheKey, slidingTime, null, cacheProtectSeconds, sourceName);
        }


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="getFunc"></param>
        /// <param name="cacheKey"></param>
        /// <param name="absoluteTime"></param>
        /// <param name="cacheProtectSeconds"></param>
        /// <returns></returns>
        public static Task<TRes> WithAbsoluteCache<TRes>(this Func<Task<TRes>> getFunc, string cacheKey, TimeSpan absoluteTime, int cacheProtectSeconds = 10, string sourceName = "default")
            where TRes : Resp
        {
            return cacheProtectSeconds==0 
                ? CacheHelper.GetOrSetAsync(cacheKey, getFunc,  null, absoluteTime,  sourceName) 
                : WithCache(getFunc, cacheKey, null, absoluteTime, cacheProtectSeconds, sourceName);
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
        public static Task<TRes> WithCache<TRes>(this Func<Task<TRes>> getFunc, string cacheKey, TimeSpan? slidingTime, TimeSpan? maxAbsoluteTime, int cacheProtectSeconds=10, string sourceName = "default")
            where TRes : Resp
        {
            if (cacheProtectSeconds==0)
            {
                return CacheHelper.GetOrSetAsync(cacheKey, getFunc, slidingTime, maxAbsoluteTime, sourceName);
            }
            return CacheHelper.GetOrSetAsync(cacheKey, getFunc, slidingTime,maxAbsoluteTime, res => !res.IsSuccess(),
                cacheProtectSeconds, sourceName);
        }
    }
}
