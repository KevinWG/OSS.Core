using OSS.Core;
using OSS.Core.Context.Attributes;
using OSS.Core.Extension.Mvc.Captcha;
using OSS.Core.Module.All.WebApi;
using System.Text.Json.Serialization;
using OSS.Core.Comp.DirConfig.Mysql;
using OSS.Core.Module.Portal;
using OSS.Tools.Config;

var builder = WebApplication.CreateBuilder(args);

ConfigHelper.Configuration = builder.Configuration;

// 使用mysql字典配置管理
builder.Services.UserMysqlDirConfigTool();

// 系统注册项
builder.Services.Register<AllWebApiGlobalStarter>();
builder.Services.AddDefaultNoneCaptchaValidator();

// mvc 
builder.Services.AddControllers(opt =>
{
    opt.AddCoreModelValidation();
    opt.AddCoreAppAuthorization(new AppAccessProvider());
    opt.AddCoreUserAuthorization(new UserAuthProvider(),new FuncAuthProvider());

}).AddJsonOptions(jsonOpt =>
{
    jsonOpt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    jsonOpt.JsonSerializerOptions.PropertyNamingPolicy   = null;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





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
