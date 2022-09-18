using OSS.Core.Comp.DirConfig.Mysql;
using OSS.Tools.Config;

var builder = WebApplication.CreateBuilder(args);

ConfigHelper.Configuration = builder.Configuration;

// ʹ��mysql�ֵ����ù���
builder.Services.UserMysqlDirConfigTool();

// Add services to the container.
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
