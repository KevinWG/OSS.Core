using OSS.Core;
using OSS.Tools.Config;
using OSS.Core.Context.Attributes;

using OSS.Core.Module.Pipeline;
using System.Text.Json.Serialization;


// ========================================  容器依赖注入   ========================================

var builder = WebApplication.CreateBuilder(args);

ConfigHelper.Configuration = builder.Configuration;  // 全局配置工具处理

builder.Services.Register<PipelineGlobalStarter>();    // 应用层全局注入 

builder.Services.AddControllers(opt =>
    {
        opt.AddCoreModelValidation();
        opt.AddCoreAppAuthorization(new AppAccessProvider());
        opt.AddCoreUserAuthorization(new UserAuthProvider(),new FuncAuthProvider());
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
