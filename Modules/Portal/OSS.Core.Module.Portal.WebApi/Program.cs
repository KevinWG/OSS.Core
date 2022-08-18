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
builder.Services.Register<PortalServiceStarter>();    // 逻辑服务层
builder.Services.Register<PortalGlobalStarter>();     // 全局注入 


builder.Services.AddControllers(opt =>
{
    opt.AddCoreModelValidation();
    opt.AddCoreAppAuthorization( new AppAccessProvider());
    opt.AddCoreUserAuthorization(new UserAuthProvider(),new FuncAuthProvider());

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
