using System;
using System.Reflection;

namespace OSS.Core.RepDapper.OrmExtention
{
   public  static class TypeExtention
    {
        public static bool IsNullableType(this Type type)
        {
            if (type.GetTypeInfo().IsGenericType)
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            return false;
        }

        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }
    }



}
