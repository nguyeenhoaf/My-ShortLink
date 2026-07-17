using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
namespace HealthCheck.Injections;

public static class ServiceCollectionExtensions
{
    public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"])
            .AddMongoDb(
                databaseNameFactory: sp => configuration["MongoDbSettings:DatabaseName"]!,
                name: "mongodb",
                tags: ["ready"])
            .AddRedis(
                configuration.GetConnectionString("Redis")!,
                name: "redis",
                tags: ["ready"]);
    }
}
