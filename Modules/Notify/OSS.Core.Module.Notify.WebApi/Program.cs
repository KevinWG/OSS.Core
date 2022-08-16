using OSS.Core;
using OSS.Core.Context.Attributes;
using OSS.Core.Extension.Mvc.Configuration;
using OSS.Core.Module.Notify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOssCoreConfiguration(builder.Configuration);

builder.Services.Register<NotifyServiceStarter>();
builder.Services.Register<NotifyAccessConfigStarter>();

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
