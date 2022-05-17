using OSS.Common.BasicMos;
using OSS.Common.Resp;

namespace OSS.Core.Extension;

    public static class ListTokenRespExtension
    {
        /// <summary>
        /// 转化通行token分页列表
        /// -附带指定列的token处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="listRes"></param>
        /// <param name="tokenColumnName">关联的key列名称</param>
        /// <param name="tokenKeySelector">对应 tokenKeyColumnName 列的 token key 选择器</param>
        /// <returns></returns>
        public static TokenListResp<TData> ToTokenList<TData>(this ListResp<TData> listRes,
            string tokenColumnName, Func<TData, string> tokenKeySelector)
        {
            return listRes.ToTokenList().AddPassToken(tokenColumnName, tokenKeySelector);
        }

        /// <summary>添加列表通行token</summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="listRes"></param>
        /// <param name="tokenColumnName">关联的key列名称</param>
        /// <param name="tokenKeySelector">对应 tokenKeyColumnName 列的 token key 选择器</param>
        /// <returns></returns>
        public static TokenListResp<TData> AddPassToken<TData>(this TokenListResp<TData> listRes,
            string tokenColumnName, Func<TData, string> tokenKeySelector)
        {
            return listRes.AddColumnToken(tokenColumnName, tokenKeySelector, x => PassTokenHelper.GenerateToken(tokenKeySelector(x)));
        }

        /// <summary>
        /// 转化为通行token分页列表
        /// 并附带默认以Id为列的token处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="listRes"></param>
        /// <returns></returns>
        public static TokenListResp<TData> ToTokenListWithIdToken<TData>(this ListResp<TData> listRes)
            where TData : BaseMo<long>
        {
            return ToTokenList(listRes, "id", GetId);
        }

        private static string GetId(BaseMo<long> mo) => mo.id.ToString();
    }

