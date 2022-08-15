using OSS.Core;
using OSS.Core.Context.Attributes;
using OSS.Core.Module.WorkFlow;
using System.Text.Json.Serialization;
using OSS.Core.Extension.Mvc.Configuration;

// ========================================  容器依赖注入   ========================================

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOssCoreConfiguration(builder.Configuration); // 全局配置处理


builder.Services.Register<WorkFlowDomainStarter>();    // 领域层启动注入

builder.Services.Register<WorkFlowServiceStarter>();   // 逻辑服务层启动注入
builder.Services.Register<WorkFlowGlobalStarter>();    // 应用层全局注入 

builder.Services.AddControllers(opt =>
    {
        opt.AddCoreModelValidation();
        opt.AddCoreAppAuthorization();
        // opt.AddCoreUserAuthorization(new DefaultUserAuthProvider(),new DefaultFuncAuthProvider());
    })
    .AddJsonOptions(jsonOpt =>
    {
        jsonOpt.JsonSerializerOptions.DefaultIgnoreCondition  = JsonIgnoreCondition.WhenWritingNull;
        jsonOpt.JsonSerializerOptions.PropertyNamingPolicy    = null;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// ========================================  中间件部分处理   ========================================

var app   = builder.Build();

var isDev = app.Environment.IsDevelopment();

if (isDev)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOssCore();
if (!isDev)
{
    app.UseOssCoreException();
}

app.MapControllers();
app.Run();
