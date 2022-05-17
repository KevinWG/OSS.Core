#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore仓储层 —— 表达式解析类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-7
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OSS.Core.ORM.Dapper.OrmExtension
{
    public class SqlExpressionVisitor
    {
        /// <summary>
        ///   表达式中的常量参数列表
        /// </summary>
        public Dictionary<string, object> parameters { get; }

        /// <summary>
        ///  表达式中的属性名称列表
        /// </summary>
        public Dictionary<string, string> properties { get; }

        public SqlExpressionVisitor()
        {
            parameters = new Dictionary<string, object>();
            properties = new Dictionary<string, string>();
        }


        /// <summary>
        ///  递归解析方法入口
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="flag"></param>
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
                case ExpressionType.Parameter:
                    VisitParameter(exp as ParameterExpression, flag);
                    break;
                case ExpressionType.Call:
                    VisitMethodCall(exp as MethodCallExpression, flag);
                    break;
                case ExpressionType.New:
                    VisitNew(exp as NewExpression, flag);
                    break;
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    VisitNewArray(exp as NewArrayExpression, flag);
                    break;
                case ExpressionType.MemberInit:
                    VisitMemberInit(exp as MemberInitExpression, flag);
                    break;
                case ExpressionType.Conditional:
                    VisitConditional(exp as ConditionalExpression);
                    break;
            }
        }

        /// <summary>
        ///   递归解析时，保留上层的IsRight状态
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="flag"></param>
        protected virtual void VisitRight(Expression exp, SqlVistorFlag flag)
        {
            var isRight = flag.is_right;
            flag.is_right = true;
            Visit(exp, flag);
            flag.is_right = isRight; //  回归
        }

        #region   不同表达式解析方法

        #region VisitMethodCall

        protected virtual void VisitMethodCall(MethodCallExpression exp, SqlVistorFlag flag)
        {
            var methodName = exp.Method.Name;
            switch (methodName) //这里其实还可以改成反射调用，不用写switch
            {
                case "Contains":
                    MethodCallLike(exp, flag);
                    break;

                    //  todo 补充其他方法
            }
        }

        private void MethodCallLike(MethodCallExpression exp, SqlVistorFlag flag)
        {
            Visit(exp.Object, flag);
            flag.Append(GetUnaryOperater(flag.unary_type));
            flag.Append(" LIKE CONCAT('%',");
            VisitRight(exp.Arguments[0], flag);
            flag.Append(",'%')");
        }

        #endregion

        protected virtual void VisitBinary(BinaryExpression exp, SqlVistorFlag flag)
        {
            var operand = GetBinaryOperater(exp.NodeType);
            if (exp.NodeType == ExpressionType.AndAlso ||
                exp.NodeType == ExpressionType.OrElse)
            {
                flag.Append("(");
                Visit(exp.Left, flag);

                flag.Append(") ").Append(operand).Append(" (");
                Visit(exp.Right, flag);
                flag.Append(")");
            }
            else
            {
                Visit(exp.Left, flag);
                flag.Append(operand);
                VisitRight(exp.Right, flag);
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
                    if (e is NewArrayExpression newArrayExpression)
                        VisitNewArray(newArrayExpression, flag);
                }
                else
                {
                    Visit(e, flag);
                }
            }
        }

        protected virtual void VisitConstant(ConstantExpression c, SqlVistorFlag flag)
        {
            var value = c.Value ?? "null";
            if (flag.is_right)
            {
                var paraName = GetCustomParaName(flag.para_pretoken); // flag.GetCustomParaName();
                flag.Append(paraName, true);

                if (c.Type.IsEnum)
                {
                    AddParameter(paraName, (int)c.Value);
                }
                else if (c.Type == typeof(bool))
                {
                    AddParameter(paraName, (bool)value ? 1 : 0);
                }
                else
                    AddParameter(paraName, value);
            }
            else
            {
                flag.Append(value.ToString());
            }
        }

        protected virtual void VisitUnary(UnaryExpression u, SqlVistorFlag flag)
        {
            var nodeType = flag.unary_type;
            flag.unary_type = u.NodeType;
            Visit(u.Operand, flag);
            flag.unary_type = nodeType;
        }

        protected virtual void VisitLambda(LambdaExpression lambda, SqlVistorFlag flag)
        {
            Visit(lambda.Body, flag);
        }

        protected virtual void VisitNew(NewExpression nex, SqlVistorFlag flag)
        {
            for (var i = 0; i < nex.Members.Count; i++)
            {
                var arg = nex.Arguments[i];
                var member = nex.Members[i];

                flag.Append(flag.GetColName(member.Name)).Append("=");
                VisitRight(arg, flag);
            }
        }

        protected virtual void VisitMember(MemberExpression exp, SqlVistorFlag flag)
        {
            if (exp.Expression != null
                && exp.Expression.NodeType == ExpressionType.Parameter)
            {
                if (flag.is_right && flag.vistor_type == SqlVistorType.Update)
                {
                    var proParaName = flag.GetParaName(exp.Member.Name);
                    flag.Append(proParaName, true);

                    AddMemberProperty(proParaName, exp.Member.Name);
                }
                else
                {
                    if (exp.Member.DeclaringType.GetTypeInfo().IsGenericType
                        && exp.Member.DeclaringType?.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        if (exp.Member.Name == "Value")
                            Visit(exp.Expression, flag);

                        if (exp.Member.Name != "HasValue") return;

                        var doesNotEqualNull = Expression.MakeBinary(ExpressionType.NotEqual, exp.Expression,
                            Expression.Constant(null));
                        Visit(doesNotEqualNull, flag);
                    }
                    else
                        flag.Append(flag.GetColName(exp.Member.Name), true);
                }
            }
            else
            {
                var value = Expression.Lambda(exp).Compile().DynamicInvoke();
                Visit(Expression.Constant(value), flag);
            }
        }



        protected virtual void VisitConditional(ConditionalExpression conditionalExpression)
        {
            // todo
        }

        protected virtual void VisitMemberInit(MemberInitExpression memberInitExpression, SqlVistorFlag flag)
        {
            //  todo
        }

        protected virtual void VisitParameter(ParameterExpression parameterExpression, SqlVistorFlag flag)
        {
            // todo
        }

        #endregion

        #region 辅助方法

        /// <summary>
        ///  添加成员属性信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pro"></param>
        private void AddMemberProperty(string name, string pro)
        {
            if (!properties.ContainsKey(name))
            {
                properties.Add(name, pro);
            }
        }

        /// <summary>
        ///  添加值对应的参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        private void AddParameter(string paraName, object value)
        {
            parameters.Add(paraName, value);
        }


        protected virtual string GetBinaryOperater(ExpressionType e)
        {
            switch (e)
            {
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.And:
                    return "&";
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
                    return string.Empty;
            }
        }


        protected virtual string GetUnaryOperater(ExpressionType e)
        {
            switch (e)
            {
                case ExpressionType.Not:
                    return "NOT";
                default:
                    return string.Empty;
            }
        }

        private int _constantParaIndex;

        /// <summary>
        ///  获取定制常量参数名称
        ///   部分参数并无指定属性，这里自定义不重复参数
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCustomParaName(string preToken)
        {
            const string parameterPre = "ConstPara";
            return string.Concat(preToken, parameterPre, _constantParaIndex++);
        }
        #endregion
    }

    /// <summary>
    ///   表达式解析标识内容体
    /// </summary>
    public class SqlVistorFlag
    {
        private readonly StringBuilder _sqlBuilder;

        public SqlVistorFlag(SqlVistorType vistorType)
        {
            vistor_type = vistorType;
            _sqlBuilder = new StringBuilder();
            unary_type = (ExpressionType)(-1);
        }

        /// <summary>
        ///   是否是表达式右侧项，决定右侧树是否做参数化处理
        /// </summary>
        public bool is_right { get; set; }

        /// <summary>
        /// 主要是MethodCall方法中解析时需要知晓上层一元运算符
        /// </summary>
        public ExpressionType unary_type { get; set; }

        /// <summary>
        ///  参数辨识符号
        /// </summary>
        public string para_pretoken { get; } = "@";

        /// <summary>
        ///  对应的SQL语句
        /// </summary>
        public string sql => _sqlBuilder.ToString().TrimEnd(GetSeparate());

        /// <summary>
        ///  解析语段类型
        /// </summary>
        public SqlVistorType vistor_type { get; set; }

        #region 语句拼接

        /// <summary>
        ///  往语句中追加内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="needCheckSep">是否需要分隔符</param>
        /// <returns></returns>
        public SqlVistorFlag Append(string content, bool needCheckSep = false)
        {
            _sqlBuilder.Append(content);
            if (needCheckSep)
            {
                _sqlBuilder.Append(GetSeparate());
            }
            return this;
        }

        /// <summary>
        ///  获取对应的参数名称
        ///    给参数添加前置符号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string GetParaName(string name)
        {
            return string.Concat(para_pretoken, name);
        }


        /// <summary>
        ///  获取属性列名称
        ///    内部补充转义字符，防止和数据库关键字名称冲突，如 `name`
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string GetColName(string name)
        {
            return string.Concat('`', name, '`');
        }


        private char GetSeparate()
        {
            switch (vistor_type)
            {
                case SqlVistorType.Update:
                    return ',';
                case SqlVistorType.Where:
                    return ' ';
                default:
                    return ' ';
            }
        }

        #endregion
    }




    public enum SqlVistorType
    {
        /// <summary>
        ///   Update 表达式解析类型
        /// </summary>
        Update,

        Where
    }

}