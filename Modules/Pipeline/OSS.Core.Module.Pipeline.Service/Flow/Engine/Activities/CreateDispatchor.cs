using OSS.Pipeline;
using OSS.Pipeline.Interface;

namespace OSS.Core.Module.Pipeline.Service.Flow.Engine.Activities
{
    internal class CreateDispatcher:BaseBranchGateway<CreateContext>
    {
        protected override bool FilterBranchCondition(CreateContext context, IPipeMeta branch)
        {
            if (context.is_manual  )
            {
                return branch.PipeCode == nameof(StartActivity);
            }

            return branch.PipeCode == FlowEngine.CreateToNextConvertor;
        }
    }
}
