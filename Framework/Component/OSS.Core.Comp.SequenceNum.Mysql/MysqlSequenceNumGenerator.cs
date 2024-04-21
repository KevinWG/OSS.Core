using OSS.Tools.Num;

namespace OSS.Core.Comp.SequenceNum;

/// <summary>
/// mysql 序列数字编号生成实现类
/// </summary>
public class MysqlSequenceNumGenerator : ISequenceNumGenerator
{
    public Task<(long start, long end)> New(string sequenceKey, int count)
    {
        throw new NotImplementedException();
    }
}