using OSS.Core;
using OSS.Core.Context.Attributes;
using OSS.Core.Module.Portal;
using System.Text.Json.Serialization;
using OSS.Core.Extension.Mvc.Captcha;
using OSS.Core.Extension.Mvc.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOssCoreConfiguration(builder.Configuration);
builder.Services.AddDefaultNoneCaptchaValidator();

// 注册内部使用服务
builder.Services.Register<PortalRepositoryStarter>(); // 仓储层
builder.Services.Register<PortalServiceStarter>();      // 逻辑服务层

// 注册外部客户端
builder.Services.Register<PortalUsedClientStarter>();   // 因为全局中间件使用，虽然在模块内部，但作为外部客户端来看


builder.Services.AddControllers(opt =>
{
    opt.AddCoreModelValidation();
    opt.AddCoreAppAuthorization();
    opt.AddCoreUserAuthorization(new DefaultUserAuthProvider(),new DefaultFuncAuthProvider());

}).AddJsonOptions(jsonOpt =>
{
    jsonOpt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    jsonOpt.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOssCore();
app.MapControllers();

app.Run();
