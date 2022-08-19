using System.Text.Json.Serialization;

using OSS.Core;
using OSS.Tools.Config;
using OSS.Core.Context.Attributes;

using OSS.Core.Module.Notify;


var builder = WebApplication.CreateBuilder(args);

ConfigHelper.Configuration = builder.Configuration;

builder.Services.Register<NotifyGlobalStarter>();

builder.Services.AddControllers(opt =>
{
    opt.AddCoreModelValidation();
    opt.AddCoreAppAuthorization(new AppAccessProvider());
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
