using System.Data;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using OSS.Common;
using OSS.Core.Context;
using OSS.Tools.Config;
using OSS.Tools.Http;

namespace OSS.Core.Module.Portal.Client.Http;

public class HttpPortalClient : IPortalClient
{
    public IOpenedPermitService Permit { get; } = SingleInstance<PermitClient>.Instance;
    public IOpenedAuthService Auth { get; } = SingleInstance<AuthClient>.Instance;
    public IOpenedSettingService Setting { get; } = SingleInstance<SettingClient>.Instance;
}
