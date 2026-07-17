using Application;
using Caching;
using Database;
using Domain.Messages;
using Infras.MongoDb.Injections;
using Infras.Redis.Injections;
using Write.Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMongoDbInfrastructure(builder.Configuration);
builder.Services.AddRedisInfrastructure(builder.Configuration);
builder.Services.AddRepositoryInfrastructure();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IUseCaseHandler<Message.Request.CreateLink, Message.Response.LinkCreated>, SaveURLHandler>();

var app = builder.Build();
await MongoInitializer.InitializeAsync(app.Services);

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
