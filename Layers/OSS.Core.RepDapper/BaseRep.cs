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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OSS.Core.RepDapper
{
    public class BaseRep
    {
        /// <summary>
        /// 根据传入要更新的表达式获取更新的列名 
        /// </summary>
        /// <param name="funExpression"></param>
        /// <returns></returns>
        private IList<string> GetColumnNames<TMoType>(Expression<Func<TMoType, object>> funExpression)
        {
            if (funExpression.Body.NodeType != ExpressionType.New)
                throw new ArgumentException("请传入正确表达式！", nameof(funExpression));

            var newE = funExpression.Body as NewExpression;

            IList<string> listColumns = new List<string>(newE.Arguments.Count);
            foreach (var arg in newE.Arguments)
            {
                if (arg.NodeType == ExpressionType.MemberAccess)
                {
                    var memExp = arg as MemberExpression;
                    if (memExp != null)
                    {
                        listColumns.Add(memExp.Member.Name);
                    }
                }
            }
            return listColumns;
        }
    }



}
