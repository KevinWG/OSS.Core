﻿using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  结束节点
/// </summary>
internal class EndActivity:BaseActivity<FlowNodeMo>
{
    protected override Task<TrafficSignal> Executing(FlowNodeMo para)
    {
        throw new NotImplementedException();
    }
}