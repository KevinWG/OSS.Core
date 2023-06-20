using System.Text.Json.Serialization;
using OSS.Core.Context.Attributes;
using OSS.Tools.Config;
using TM.Modules.Files;

var builder = WebApplication.CreateBuilder(args);

ConfigHelper.Configuration = builder.Configuration;


// mvc 
builder.Services.AddControllers(opt =>
{
    opt.AddCoreModelValidation();
    opt.AddCoreAppAuthorization(new AppAccessProvider());
    opt.AddCoreTenantAuthorization(new TenantAuthProvider());
    opt.AddCoreUserAuthorization(new UserAuthProvider(), new FuncAuthProvider());

}).AddJsonOptions(jsonOpt =>
{
    jsonOpt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    jsonOpt.JsonSerializerOptions.PropertyNamingPolicy   = null;
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
app.UseAuthorization();

app.MapControllers();

app.Run();
