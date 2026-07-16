using Application;
using Domain.Messages;
using Infras.MongoDb.Injections;
using Infras.Redis.Injections;
using Redirect.Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMongoDbInfrastructure(builder.Configuration);
builder.Services.AddRedisInfrastructure(builder.Configuration); 
builder.Services.AddRepositoryInfrastructure();
builder.Services.AddScoped<IUseCaseHandler<Message.Request.GetOriginalUrl, Message.Response.OriginalUrlGot>, GetOriginalHandler>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
