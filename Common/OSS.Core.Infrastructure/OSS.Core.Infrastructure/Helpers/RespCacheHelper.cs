using OSS.Common.BasicMos.Resp;
using OSS.Tools.Cache;
using System;
using System.Threading.Tasks;

namespace OSS.Core.Infrastructure.Helpers
{
    /// <summary>
    ///  返回结果(继承Resp)的对象缓存辅助类  
    /// </summary>
    public static class RespCacheHelper
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="getFunc"></param>
        /// <param name="cacheKey"></param>
        /// <param name="absoluteTime"></param>
        /// <param name="cacheProtectSeconds"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Task<TRes> GetOrSetAbsoluteAsync<TRes>(string cacheKey, Func<Task<TRes>> getFunc, 
            TimeSpan absoluteTime, int cacheProtectSeconds = 10, string sourceName = "default")
            where TRes : Resp, new()
        {
            return GetOrSetAsync(cacheKey, getFunc,  null, absoluteTime, cacheProtectSeconds, sourceName);
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
        public static Task<TRes> GetOrSetAsync<TRes>(string cacheKey, Func<Task<TRes>> getFunc, TimeSpan slidingTime,
            int cacheProtectSeconds = 10, string sourceName = "default")
            where TRes : Resp, new()
        {
            return GetOrSetAsync(cacheKey, getFunc, slidingTime, null, cacheProtectSeconds, sourceName);
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
        public static Task<TRes> GetOrSetAsync<TRes>(string cacheKey, Func<Task<TRes>> getFunc,  TimeSpan? slidingTime,
            TimeSpan? maxAbsoluteTime, int cacheProtectSeconds = 10, string sourceName = "default")
            where TRes : Resp, new()
        {
            if (cacheProtectSeconds == 0)
            {
                return GetOrSetAsync(cacheKey, getFunc, slidingTime, maxAbsoluteTime, sourceName);
            }
            return CacheHelper.GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions() { sliding_expiration = slidingTime, absolute_expiration_relative_to_now = maxAbsoluteTime }, res => !res.IsSuccess(),
                cacheProtectSeconds, sourceName);
        }

        /// <summary>
        ///  根据结果清理缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Task<bool> RemoveAsync<TRes>(string cacheKey, string sourceName = "default")
        {
            return CacheHelper.RemoveAsync(cacheKey, sourceName);
        }

        /// <summary>
        ///  根据结果清理缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="cacheKeys"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Task<bool> RemoveAsync<TRes>(string[] cacheKeys, string sourceName = "default")
        {
            return CacheHelper.RemoveAsync(cacheKeys, sourceName);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="getFunc"></param>
        /// <param name="cacheKey"></param>
        /// <param name="slidingTime"></param>
        /// <param name="maxAbsoluteTime"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        private static async Task<TRes> GetOrSetAsync<TRes>(string cacheKey, Func<Task<TRes>> getFunc,
            TimeSpan? slidingTime, TimeSpan? maxAbsoluteTime, string sourceName = "default")
            where TRes : Resp, new()
        {
            var obj = await CacheHelper.GetAsync<TRes>(cacheKey, sourceName);
            if (obj != null)
                return obj;

            if (getFunc == null)
                return new TRes().WithResp(RespTypes.UnKnowOperate, "未实现获取方法！");

            var data = await getFunc.Invoke();
            if (data == null || !data.IsSuccess())
                return data;

            await CacheHelper.SetAsync(cacheKey, data,new CacheTimeOptions(){sliding_expiration = slidingTime,absolute_expiration_relative_to_now = maxAbsoluteTime } , sourceName);
            return data;
        }
    }
}
