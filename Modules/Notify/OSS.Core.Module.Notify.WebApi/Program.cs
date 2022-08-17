using System.Text.Json.Serialization;

using OSS.Core;
using OSS.Core.Context.Attributes;
using OSS.Core.Extension.Mvc.Configuration;

using OSS.Core.Module.Notify;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOssCoreConfiguration(builder.Configuration);

builder.Services.Register<NotifyServiceStarter>();
builder.Services.Register<NotifyGlobalStarter>();

builder.Services.AddControllers(opt =>
{
    opt.AddCoreModelValidation();
    opt.AddCoreAppAuthorization();
    opt.AddCoreUserAuthorization(new UserAuthProvider(), new FuncAuthProvider());

}).AddJsonOptions(jsonOpt =>
{
    jsonOpt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    jsonOpt.JsonSerializerOptions.PropertyNamingPolicy   = null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseOssCore();

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
