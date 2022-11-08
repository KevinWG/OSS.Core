using System.Text.Json.Serialization;
using OSS.Core.Context.Attributes;
using OSS.Core.Extension.Mvc.Captcha;
using WeApi.Test;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultNoneCaptchaValidator();

// Add services to the container.
// mvc 
builder.Services.AddControllers(opt =>
{
    opt.AddCoreModelValidation();
    opt.AddCoreAppAuthorization(new AppAccessProvider(),new TenantAuthProvider());

}).AddJsonOptions(jsonOpt =>
{
    jsonOpt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    jsonOpt.JsonSerializerOptions.PropertyNamingPolicy   = null;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app   = builder.Build();
var isDev = app.Environment.IsDevelopment();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOssCore();
if (!isDev)
{
    app.UseOssCoreException();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
