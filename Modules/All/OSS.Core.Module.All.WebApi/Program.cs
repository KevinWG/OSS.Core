using OSS.Core;
using OSS.Core.Context.Attributes;
using OSS.Core.Extension.Mvc.Captcha;
using OSS.Core.Module.All.WebApi;
using System.Text.Json.Serialization;
using OSS.Core.Extension.Mvc.Configuration;
using OSS.Core.Module.Portal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOssCoreConfiguration(builder.Configuration);

// ÏµÍ³×¢²áÏî
builder.Services.Register<AllWebApiStarter>();
builder.Services.Register<AllWebUsedClientStarter>();
builder.Services.AddDefaultNoneCaptchaValidator();

// mvc 
builder.Services.AddControllers(opt =>
{
    opt.AddCoreModelValidation();
    opt.AddCoreAppAuthorization(new AppAuthDefaultProvider());
    opt.AddCoreUserAuthorization(new LocalUserAuthProvider(),new LocalFunAuthProvider());
}).AddJsonOptions(jsonOpt =>
{
    jsonOpt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    jsonOpt.JsonSerializerOptions.PropertyNamingPolicy   = null;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseOssCore(new CoreContextOption()
{
    JSRequestHeaderName = "x-core-app"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseOssCoreException();
}


app.MapControllers();
app.Run();
