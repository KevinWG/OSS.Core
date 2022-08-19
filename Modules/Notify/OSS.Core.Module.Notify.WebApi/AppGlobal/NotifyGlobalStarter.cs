
namespace OSS.Core.Module.Notify;

/// <summary>
///  通知模块涉及秘钥配置等处理
/// </summary>
public class NotifyGlobalStarter : AppStarter
{
    public override void Start(IServiceCollection service)
    {
        service.Register<NotifyServiceStarter>();
    }
}
