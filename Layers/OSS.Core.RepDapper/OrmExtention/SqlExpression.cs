using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OSS.Core.RepDapper.OrmExtention
{
    public class SqlExpression 
    {
        private readonly Dictionary<string, object> parameters;
        private readonly Dictionary<string,PropertyInfo> properties;
        
        public SqlExpression()
        {
            parameters = new Dictionary<string, object>();
            properties = new Dictionary<string, PropertyInfo>();
        }

        public virtual void Visit(Expression exp, SqlVistorFlag flag)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Lambda:
                    VisitLambda(exp as LambdaExpression, flag);
                    break;
                case ExpressionType.MemberAccess:
                    VisitMember(exp as MemberExpression, flag);
                    break;
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    VisitBinary(exp as BinaryExpression, flag);
                    break;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    VisitUnary(exp as UnaryExpression, flag);
                    break;
                case ExpressionType.Constant:
                    VisitConstant(exp as ConstantExpression, flag);
                    break;
                //case ExpressionType.Parameter:
                //    return VisitParameter(exp as ParameterExpression);
                //case ExpressionType.Call:
                //    return VisitMethodCall(exp as MethodCallExpression);
                case ExpressionType.New:
                    VisitNew(exp as NewExpression, flag);
                    break;
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    VisitNewArray(exp as NewArrayExpression, flag);
                    break;
                //case ExpressionType.MemberInit:
                //    return VisitMemberInit(exp as MemberInitExpression);
                //case ExpressionType.Conditional:
                //    return VisitConditional(exp as ConditionalExpression);
            }
        }


        /// <summary>
        ///   递归解析时，保留上层的IsRight状态
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="flag"></param>
        private void VisitRight(Expression exp, SqlVistorFlag flag)
        {
            var isRight = flag.IsRight;
            flag.IsRight = true;
            Visit(exp, flag);
            flag.IsRight = isRight; //  回归
        }

        private void VisitBinary(BinaryExpression exp,SqlVistorFlag flag)
        {
            var operand = GetBinaryOperant(exp.NodeType);
            if (exp.NodeType==ExpressionType.AndAlso||
                exp.NodeType==ExpressionType.OrElse)
            {
                flag.Append(" (");
                Visit(exp.Left,flag);
                flag.Append(") ").Append(operand).Append(" (");
                Visit(exp.Right,flag);
                flag.Append(")");
            }
            else
            {
                Visit(exp.Left,flag);
                flag.Append(operand);
                VisitRight(exp.Right,flag);
            }
        }

        protected void VisitNewArray(NewArrayExpression na, SqlVistorFlag flag)
        {
            var original = na.Expressions;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                var e = original[i];
                if (e.NodeType == ExpressionType.NewArrayInit ||
                    e.NodeType == ExpressionType.NewArrayBounds)
                {
                    var newArrayExpression = e as NewArrayExpression;
                    if (newArrayExpression != null)
                       VisitNewArray(newArrayExpression,flag);
                }
                else
                {
                    Visit(e,flag);
                }
            }
        }
        
        protected virtual void VisitConstant(ConstantExpression c,SqlVistorFlag flag)
        {
            var value = c.Value ?? "null";
            if (flag.IsRight)
            {
                const string parameterPre = "ConstPara";
                var paraName = GetParaName(parameterPre, true);
                flag.Append(paraName);
                AddParameter(paraName, value);
            }
            else
            {
                flag.Append(value.ToString());
            }

        }

        protected virtual void VisitUnary(UnaryExpression u, SqlVistorFlag flag)
        {
             Visit(u.Operand, flag);
        }
        
        protected virtual void VisitLambda(LambdaExpression lambda,SqlVistorFlag flag)
        {
             Visit(lambda.Body, flag);
        }

        protected virtual void VisitNew(NewExpression nex, SqlVistorFlag flag)
        {
            for (var i = 0; i < nex.Members.Count; i++)
            {
                var arg = nex.Arguments[i];
                var member = nex.Members[i];
                flag.Append(string.Concat(member.Name, "="), true);

                VisitRight(arg, flag);
            }
        }

        protected virtual void VisitMember(MemberExpression exp, SqlVistorFlag flag)
        {
            if (flag.IsRight)
            {
                var proParaName = GetParaName(exp.Member.Name);
                flag.Append(proParaName);
                AddMemberProperty(proParaName, exp.Member.DeclaringType.GetProperty(exp.Member.Name));
            }
            else
            {
                flag.Append(exp.Member.Name);
            }
        }




        #region 辅助方法
        /// <summary>
        ///  添加成员属性信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pro"></param>
        private void AddMemberProperty(string name,PropertyInfo pro)
        {
            if (!properties.ContainsKey(name))
            {
                properties.Add(name,pro);
            }
        }

        /// <summary>
        ///  添加参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        private void AddParameter(string paraName, object value)
        {
            parameters.Add(paraName, value);
        }

        private const string prefixToken = "@";
        private int constantParaIndex = 0;
        private string GetParaName(string nameWithNoPre,bool isConstant=false)
        {
            var name = string.Concat(prefixToken, nameWithNoPre);
            if (!isConstant)
                return name;
            return  string.Concat(name, constantParaIndex++);
        }

        protected virtual string GetBinaryOperant(ExpressionType e)
        {
            switch (e)
            {
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.And:
                    return "|";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Or:
                    return "|";
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.Subtract:
                    return "-";
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Modulo:
                    return "MOD";
                case ExpressionType.Coalesce:
                    return "COALESCE";
                default:
                    return e.ToString();
            }
        }
        #endregion
    }

    /// <summary>
    ///   表达式解析标识内容体
    /// </summary>
    public class SqlVistorFlag
    {
        public SqlVistorFlag(SqlVistorType vistorType)
        {
            VistorType = vistorType;
            sqlBuilder = new StringBuilder();
        }

        public SqlVistorType VistorType { get; set; }

        /// <summary>
        ///   是否是表达式右侧项，决定Member是否做参数化处理
        /// </summary>
        public bool IsRight { get; set; }

        #region 语句拼接

        private bool isStarted = false;
        public string Sql => sqlBuilder.ToString();

        private readonly StringBuilder sqlBuilder;

        public SqlVistorFlag Append(string content,bool needCheckSep=false)
        {
            if (!isStarted)
                isStarted = true;
            else
            {
                if (needCheckSep)
                {
                    sqlBuilder.Append(GetSeparate());
                }
            }

            sqlBuilder.Append(content);
            return this;
        }

        public string GetSeparate()
        {
            switch (VistorType)
            {
                case SqlVistorType.InsertOrUpdate:
                    return ",";

                case SqlVistorType.Where:
                    return " ";
            }
            return string.Empty;
        }

        #endregion
    }


    public enum SqlVistorType
    {
        /// <summary>
        ///   Insert和Update 表达式解析类型
        /// </summary>
        InsertOrUpdate,

        Where
    }

}
