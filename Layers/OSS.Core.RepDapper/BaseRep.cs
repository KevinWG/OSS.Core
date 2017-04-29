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
using System.Reflection;
using System.Reflection.Emit;

namespace OSS.Core.RepDapper
{

    //  1. 把表达式生成的字符串缓存起来
    //  2. 反射扩展添加指定类型到匿名对象的生成
    public class BaseRep
    {
        protected readonly string m_TableName = string.Empty;



    }

    public static class SqlMapper
    {


    

        /// <summary>
        /// 根据传入要更新的表达式获取更新的列名 
        /// </summary>
        /// <param name="funExpression"></param>
        /// <returns></returns>
        private static IList<string> GetColumnNames<TMoType>(Expression<Func<TMoType, object>> funExpression)
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


        /// <summary>
        /// 获取类型下指定部分字段的委托方法
        /// </summary>
        /// <typeparam name="MType"></typeparam>
        /// <param name="key"></param>
        /// <param name="proList"></param>
        /// <returns></returns>
        private static Func<MType, Dictionary<string, object>> CreateDicDeleMothed<MType>(string key,IReadOnlyCollection<PropertyInfo> proList)
        {
            var moType = typeof(MType);


            var dicType = typeof(Dictionary<string, object>);
            var dirAddMethod = dicType.GetMethod("Add");
            var dicCtor = dicType.GetConstructor(new[] {typeof(int)});

            var conFunM = new DynamicMethod(string.Concat("ConverTofunc_", key),
                MethodAttributes.Public | MethodAttributes.Static,
                CallingConventions.Standard, dicType, new[] {moType}, moType, false);

            var ilT = conFunM.GetILGenerator();
            ilT.DeclareLocal(dicType);

            // 申明局部变量
            ilT.Emit(OpCodes.Nop);
            ilT.Emit(OpCodes.Ldc_I4, proList.Count);
            ilT.Emit(OpCodes.Newobj, dicCtor);

            ilT.Emit(OpCodes.Stloc_0);
            foreach (var pro in proList)
            {
                ilT.Emit(OpCodes.Nop);
                ilT.Emit(OpCodes.Ldloc_0);

                ilT.Emit(OpCodes.Ldstr, pro.Name);
                ilT.Emit(OpCodes.Ldarg_0);
                ilT.Emit(OpCodes.Callvirt, pro.GetGetMethod());
                if (pro.PropertyType.GetTypeInfo().IsValueType)
                {
                    ilT.Emit(OpCodes.Box, pro.PropertyType);
                }

                ilT.Emit(OpCodes.Callvirt, dirAddMethod);
            }
            ilT.Emit(OpCodes.Nop);
            ilT.Emit(OpCodes.Ldloc_0);
            ilT.Emit(OpCodes.Ret);

            return
                conFunM.CreateDelegate(typeof(Func<MType, Dictionary<string, object>>)) as
                    Func<MType, Dictionary<string, object>>;
        }

        #region  通过自定义type实现，但.net standard 暂时不支持AppDomain，无法动态创建程序集,仅供参考

        //private static Func<MType, object> CreateDelegateMothed<MType>(string key)
        //{
        //    var moType = typeof(MType);
        //    var proList = moType.GetProperties();
        //    Type tCustom = NewMethod(proList, key);

        //    DynamicMethod conFunM = new DynamicMethod(string.Concat("ConverTofunc_", key),
        //        MethodAttributes.Public | MethodAttributes.Static,
        //        CallingConventions.Standard, typeof(object), new[] { moType }, moType, false);
        //    var ctor = tCustom.GetConstructor(proList.Select(p => p.PropertyType).ToArray());

        //    var ilT = conFunM.GetILGenerator();
        //    ilT.DeclareLocal(tCustom);

        //    ilT.Emit(OpCodes.Nop);
        //    foreach (var pro in proList)
        //    {
        //        ilT.Emit(OpCodes.Ldarg_0);
        //        ilT.Emit(OpCodes.Callvirt, pro.GetGetMethod());
        //    }

        //    ilT.Emit(OpCodes.Newobj, ctor);
        //    ilT.Emit(OpCodes.Stloc_0);
        //    ilT.Emit(OpCodes.Ldloc_0);

        //    ilT.Emit(OpCodes.Nop);
        //    ilT.Emit(OpCodes.Ret);

        //    //conFunM.DefineParameter(1, ParameterAttributes.None, "mo");

        //    Func<MType, object> uls =
        //        conFunM.CreateDelegate(typeof(Func<MType, object>)) as Func<MType, object>;
        //    return uls;
        //}

        //private static Type NewMethod(IReadOnlyList<PropertyInfo> proList, string perName)
        //{
        //    var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(string.Concat(perName, "DapperAsserbly")),
        //                    AssemblyBuilderAccess.Run);
        //    var moduleBuilder = assemblyBuilder.DefineDynamicModule(string.Concat(perName, "DapperModule"));
        //    var typeBuilder = moduleBuilder.DefineType(string.Concat(perName, "DapperType"), TypeAttributes.Public);
        //    var ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, proList.Select(p => p.PropertyType).ToArray());

        //    var ctorIL = ctorBuilder.GetILGenerator();

        //    ctorIL.Emit(OpCodes.Ldarg_0);
        //    ctorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));

        //    for (var i = 0; i < proList.Count; i++)
        //    {
        //        var pro = proList[i];
        //        ctorIL.Emit(OpCodes.Nop);
        //        ctorIL.Emit(OpCodes.Ldarg_0);
        //        if (i < 3)
        //        {
        //            ctorIL.Emit(i == 0 ? OpCodes.Ldarg_1 : (i == 1 ? OpCodes.Ldarg_2 : OpCodes.Ldarg_3));
        //        }
        //        else
        //            ctorIL.Emit(OpCodes.Ldarg_S, i + 1);

        //        FieldBuilder fieldBuilder = typeBuilder.DefineField(string.Concat("_", pro.Name), pro.PropertyType, FieldAttributes.Private);
        //        ctorIL.Emit(OpCodes.Stfld, fieldBuilder);

        //        PropertyBuilder pbBulider = typeBuilder.DefineProperty(pro.Name, PropertyAttributes.None, pro.PropertyType, Type.EmptyTypes);
        //        MethodBuilder pbGetAccessor = typeBuilder.DefineMethod(string.Concat("get_", pro.Name), MethodAttributes.Public,
        //            pro.PropertyType, Type.EmptyTypes);

        //        ILGenerator nameGetIL = pbGetAccessor.GetILGenerator();
        //        nameGetIL.Emit(OpCodes.Ldarg_0);
        //        nameGetIL.Emit(OpCodes.Ldfld, fieldBuilder);
        //        nameGetIL.Emit(OpCodes.Ret);

        //        pbBulider.SetGetMethod(pbGetAccessor);
        //    }

        //    ctorIL.Emit(OpCodes.Ret);
        //    return typeBuilder.CreateType();
        //}

        #endregion
    }

    public class OperateCacheInfo<MType>
    {
        /// <summary>
        /// 当前操作要执行的语句
        /// </summary>
        public string Sql { get; set; }
        /// <summary>
        ///  转化生成参数的委托
        /// </summary>
        public Func<MType, Dictionary<string, object>> ParateFunc { get; set; }

        //public 
    }

}






