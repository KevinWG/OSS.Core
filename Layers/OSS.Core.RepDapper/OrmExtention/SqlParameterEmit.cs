
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore仓储层 —— 参数化处理委托Emit生成辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-4
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace OSS.Core.RepDapper.OrmExtention
{
    internal class SqlParameterEmit
    {
        /// <summary>
        /// 获取类型下指定部分字段的委托方法
        /// </summary>
        /// <typeparam name="MType"></typeparam>
        /// <param name="proList"></param>
        /// <returns></returns>
        public static Func<object, Dictionary<string, object>> CreateDicDeleMothed<MType>( IEnumerable<PropertyInfo> proList)
        {
            var moType = typeof(MType);

            var dicType = typeof(Dictionary<string, object>);
            var dirAddMethod = dicType.GetMethod("Add");
            var dicCtor = dicType.GetConstructor(new[] { typeof(int) });

            var conFunM = new DynamicMethod(string.Concat("ConverTofunc_", Guid.NewGuid().GetHashCode()),
                MethodAttributes.Public | MethodAttributes.Static,
                CallingConventions.Standard, dicType, new[] { typeof(object) }, moType, false);

            var ilT = conFunM.GetILGenerator();
            ilT.DeclareLocal(dicType);

            ilT.Emit(OpCodes.Nop);
            ilT.Emit(OpCodes.Ldc_I4, proList.Count());
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
                    if (pro.PropertyType == typeof(bool))
                    {
                        var boolJumpLabs = new[]
                        {
                            ilT.DefineLabel(),
                            ilT.DefineLabel()
                        };
                        ilT.Emit(OpCodes.Brtrue_S, boolJumpLabs[0]);

                        ilT.Emit(OpCodes.Ldc_I4_0);
                        ilT.Emit(OpCodes.Br_S, boolJumpLabs[1]);

                        ilT.MarkLabel(boolJumpLabs[0]);
                        ilT.Emit(OpCodes.Ldc_I4_1);
                        ilT.MarkLabel(boolJumpLabs[1]);
                        ilT.Emit(OpCodes.Box, typeof(int));
                    }
                    else
                    {
                        ilT.Emit(OpCodes.Box, pro.PropertyType);
                    }
                }
                ilT.Emit(OpCodes.Callvirt, dirAddMethod);
            }
            ilT.Emit(OpCodes.Nop);
            ilT.Emit(OpCodes.Ldloc_0);
            ilT.Emit(OpCodes.Ret);

            return
                conFunM.CreateDelegate(typeof(Func<object, Dictionary<string, object>>)) as
                    Func<object, Dictionary<string, object>>;
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
}
