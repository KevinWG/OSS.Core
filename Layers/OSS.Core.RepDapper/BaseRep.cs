#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore仓储层 —— 仓储基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-21
*       
*****************************************************************************/

#endregion

using System;
using System.Data;
using MySql.Data.MySqlClient;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Modules.LogModule;
using OSS.Core.DomainMos;

namespace OSS.Core.RepDapper
{

    //  1. 把表达式生成的字符串缓存起来
    //  2. 反射扩展添加指定类型到匿名对象的生成
    public class BaseRep<TType, IdType>
        where TType: BaseMo<IdType>,new() 
    {
        protected  string m_TableName;


        protected readonly string writeConnectionString;
        protected readonly string readeConnectionString;

        public BaseRep(string writeConnectionStr=null,string readeConnectionStr=null)
        {
            writeConnectionString = writeConnectionStr;
            readeConnectionString = readeConnectionStr;
        }

        //public virtual ResultIdMo Insert(TType mo)
        //{

        //}


        #region 底层基础读写分离封装
        /// <summary>
        /// 执行写数据库操作
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal RType ExcuteWrite<RType>(Func<IDbConnection, RType> func) where RType : ResultMo, new()
            => Execute(func, true);
   
        /// <summary>
        ///  执行读数据库操作
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected internal ResultMo<RType> ExcuteReade<RType>(Func<IDbConnection, RType> func) => Execute(con =>
        {
            var res = func(con);
            return res != null ? new ResultMo<RType>(res) : new ResultMo<RType>(ResultTypes.ObjectNull, "未发现相关数据！");
        }, false);

        private  RType Execute<RType>(Func<IDbConnection, RType> func,bool isWrite)
            where RType : ResultMo,new ()
        {
            var t = default(RType);
            try
            {
                using (var con=new MySqlConnection(isWrite? writeConnectionString: readeConnectionString))
                {
                    t= func(con);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                throw e;
#else
                 LogUtil.Error(string.Concat("数据库操作错误，详情：", e.Message, "\r\n", e.StackTrace), "DataRepConnectionError",
                    "DapperRep");
                t = new RType
                {
                    Ret = (int) ResultTypes.InnerError,
                    Message = "数据更新出错！"
                };
                return t;
#endif
            }
            return t ?? new RType() {Ret =(int)ResultTypes.ObjectNull,Message = "未发现对应结果响应"};
        }
        #endregion

    }



}






