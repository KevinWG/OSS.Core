using OSS.Core.Context.Attributes;

using OSS.Core.Portal.Repository;
using OSS.Core.Portal.Service;



var builder = WebApplication.CreateBuilder(args);


builder.Services.Register<PortalRepositoryStarter>();
builder.Services.Register<PortalServiceStarter>();




var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOssCore();
app.Run();
