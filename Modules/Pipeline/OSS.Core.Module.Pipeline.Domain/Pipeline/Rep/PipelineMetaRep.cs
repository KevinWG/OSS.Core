namespace OSS.Core.Module.Pipeline;

/// <summary>
///  PipelineMeta 对象仓储
/// </summary>
public class PipelineMetaRep : BasePipelineRep<MetaMo,long> 
{
    /// <inheritdoc />
    public PipelineMetaRep() : base(PipelineConst.RepTables.PipelineMeta)
    {
    }
}
