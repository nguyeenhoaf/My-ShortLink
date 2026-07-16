using Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Database;

public static class MongoInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        BsonClassMap.RegisterClassMap<LinkDocument>(cm =>
        {
            cm.AutoMap();
            cm.MapIdProperty(x => x.Id)
                              .SetIdGenerator(StringObjectIdGenerator.Instance)
                              .SetSerializer(new StringSerializer(BsonType.ObjectId));
        });

        await MongoIndexes.CreateAsync(services);
    }
}
