using System.Text.Json.Serialization;

using OSS.Core;
using OSS.Tools.Config;
using OSS.Core.Context.Attributes;

using OSS.Core.Extension.Mvc.Captcha;
using OSS.Core.Module.Portal;


var builder = WebApplication.CreateBuilder(args);

ConfigHelper.Configuration = builder.Configuration;

builder.Services.Register<PortalGlobalStarter>(); // È«¾Ö×¢Èë
builder.Services.AddDefaultNoneCaptchaValidator();

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
