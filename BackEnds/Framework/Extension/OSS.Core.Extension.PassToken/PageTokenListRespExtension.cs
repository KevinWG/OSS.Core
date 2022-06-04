using OSS.Common.Resp;
using OSS.Core.Domain;


namespace OSS.Core.Extension;
public static class PageTokenListRespExtension
    {
        /// <summary>
        /// 转化通行token分页列表
        /// -附带指定列的token处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="pageList"></param>
        /// <param name="tokenColumnName">关联的key列名称</param>
        /// <param name="tokenKeySelector">对应 tokenKeyColumnName 列的 token key 选择器</param>
        /// <returns></returns>
        public static PageTokenListResp<TData> ToPageTokenList<TData>(this PageListResp<TData> pageList,
            string tokenColumnName, Func<TData, string> tokenKeySelector)
        {
            return pageList.ToPageTokenList().AddPassToken(tokenColumnName, tokenKeySelector);
        }

        /// <summary>添加列表通行token</summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="pageList"></param>
        /// <param name="tokenColumnName">关联的key列名称</param>
        /// <param name="tokenKeySelector">对应 tokenKeyColumnName 列的 token key 选择器</param>
        /// <returns></returns>
        public static PageTokenListResp<TData> AddPassToken<TData>(this PageTokenListResp<TData> pageList,
            string tokenColumnName, Func<TData, string> tokenKeySelector)
        {
            return pageList.AddColumnToken(tokenColumnName, tokenKeySelector,
                x => PassTokenHelper.GenerateToken(tokenKeySelector(x)));
        }

        /// <summary>
        /// 转化为通行token分页列表
        /// 并附带默认以Id为列的token处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="pageList"></param>
        /// <returns></returns>
        public static PageTokenListResp<TData> ToPageTokenListWithIdToken<TData>(this PageListResp<TData> pageList)
            where TData : BaseMo<long>
        {
            return ToPageTokenList(pageList, "id", GetId);
        }

        private static string GetId(BaseMo<long> mo) => mo.id.ToString();
    }
