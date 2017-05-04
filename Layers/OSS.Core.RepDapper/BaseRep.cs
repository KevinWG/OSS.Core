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

using OSS.Core.DomainMos;

namespace OSS.Core.RepDapper
{

    //  1. 把表达式生成的字符串缓存起来
    //  2. 反射扩展添加指定类型到匿名对象的生成
    public class BaseRep<TType, IdType>
        where TType: BaseMo<IdType>,new() 
    {
        protected  string m_TableName;

        //  插入，判断是否返回id,   todo type全称，是否自增长，连接串 缓存
        //  todo  如果是枚举值和布尔值，需要替换成数字   

    }



}






