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

namespace OSS.Core.DomainMos
{
    /// <summary>
    /// 仓储单例及注入实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Rep<T>
    {
        public static T Instance { get; private set; }

        public static void Set<TRep>()
            where TRep : T, new()
        {
            if (Instance == null)
                Instance = new TRep();
        }
    }
}
