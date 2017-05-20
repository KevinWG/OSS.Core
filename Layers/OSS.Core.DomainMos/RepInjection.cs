#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 仓储单例及注入实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-18
*       
*****************************************************************************/

#endregion

using System;

namespace OSS.Core.DomainMos
{
    /// <summary>
    /// 仓储单例及注入实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Rep<T>
    {
        public static T Instance => instanceCreater();

        private static Func<T> instanceCreater;
        private static T instance;

        public static void Set<TRep>()
            where TRep : T, new()
        {
            if (instanceCreater == null)
                instanceCreater = () =>
                {
                    if (instance != null) return instance;
                    return instance = new TRep(); 
                };
        }
    }
}
