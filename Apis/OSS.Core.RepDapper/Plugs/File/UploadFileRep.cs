using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.RepDapper.Plugs.File.Mos;

namespace OSS.Core.RepDapper.Plugs.File
{
    public class UploadFileRep : BaseTenantRep<UploadFileRep, UploadFileMo>
    {

        protected override string GetTableName()
        {
            return "p_file_uploadfiles";
        }

        /// <summary>
        ///  获取列表信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PageListResp<UploadFileMo>> GetPageList(SearchReq search)
        {
            var whereSql = BuildSimpleSearchWhereSql(search.filters,out var sqlParas);
            var offCount = search.GetStartRow();

            var selectSql = string.Concat("select * from ", TableName,whereSql, defaultOrderSql,
                " limit ", search.size, " offset ", offCount);
            var totalSql = string.Concat("select count(1) from ", TableName, whereSql);

            return await GetPageList(selectSql, sqlParas, totalSql);
        }


        protected override string BuildSimpleSearchWhereSqlByFilterItem(string key, string value, Dictionary<string, object> para)
        {
            switch (key)
            {
                case "owner_uid":
                    para.Add("@owner_uid", value);
                    return " and \"owner_uid\"=@owner_uid";

                case "file_type":
                    para.Add("@file_type", value);
                    return " and \"type\"=@file_type";

                default:
                    return base.BuildSimpleSearchWhereSqlByFilterItem(key, value, para);
            }

        }


    }
}
