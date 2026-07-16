using Domain;
using Domain.Constants;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace Database;

public static class MongoIndexes
{
    public static async Task CreateAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

        var links = database.GetCollection<LinkDocument>(nameof(CollectionNames.Link));
        await links.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<LinkDocument>(
                Builders<LinkDocument>.IndexKeys.Ascending(x => x.ShortCode),
                new CreateIndexOptions
                {
                    Unique = true,
                    Name = "IX_ShortCode"
                })
        });
    }
}
