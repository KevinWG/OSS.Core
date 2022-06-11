using OSS.Core.Context.Attributes;

using OSS.Core.Portal.Repository;
using OSS.Core.Portal.Service;
using OSS.Core.WebApis.App_Codes.AuthProviders;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.Register<PortalRepositoryStarter>();
builder.Services.Register<PortalServiceStarter>();



var appOption = new AppAuthOption()
{
    AppProvider = new AppAuthProvider(),
};
var userOption = new UserAuthOption()
{
    UserProvider = new UserAuthProvider()
};

builder.Services.AddControllers(opt =>
    {
        opt.Filters.Add(new AppAuthAttribute(appOption));
        opt.Filters.Add(new UserAuthAttribute(userOption));
    })
    .AddJsonOptions(jsonOpt =>
    {
        jsonOpt.JsonSerializerOptions.IgnoreNullValues     = true;
        jsonOpt.JsonSerializerOptions.PropertyNamingPolicy = null;
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOssCore();
app.Run();
