using Application;
using Caching;
using Database;
using Domain.Messages;
using HealthCheck.Constants;
using HealthCheck.Injections;
using Infras.MongoDb.Injections;
using Infras.Redis.Injections;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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
builder.Services.AddHealthChecks(builder.Configuration);

var app = builder.Build();
await MongoInitializer.InitializeAsync(app.Services);
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapHealthChecks(HealthCheckConstants.API_ENDPOINT, new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains(HealthCheckConstants.API_TAG)
});
app.MapHealthChecks(HealthCheckConstants.DATABASE_ENDPOINT, new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains(HealthCheckConstants.DATABASE_TAG)
});
app.Run();
