using OSS.Core.Context.Attributes;
using OSS.Core.Extension.Mvc.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOssCoreConfiguration(builder.Configuration);

builder.Services.AddMvc(opt =>
{
    opt.AddCoreModelValidation();
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

app.Run();
