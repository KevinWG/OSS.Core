using OSS.Core;
using OSS.Core.Context.Attributes;
using OSS.Core.Module.Portal;
using System.Text.Json.Serialization;
using OSS.Core.Extension.Mvc.Captcha;
using OSS.Core.Extension.Mvc.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOssCoreConfiguration(builder.Configuration);
builder.Services.AddDefaultNoneCaptchaValidator();

// ע���ڲ�ʹ�÷���
builder.Services.Register<PortalRepositoryStarter>(); // �ִ���
builder.Services.Register<PortalServiceStarter>();      // �߼������

// ע���ⲿ�ͻ���
builder.Services.Register<PortalUsedClientStarter>();   // ��Ϊȫ���м��ʹ�ã���Ȼ��ģ���ڲ�������Ϊ�ⲿ�ͻ�������


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
