using ApiGateway.Injections;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxyApiGateway(builder.Configuration);

var app = builder.Build();
app.MapReverseProxy();
app.Run();
