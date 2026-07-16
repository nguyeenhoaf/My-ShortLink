using Application;
using Caching;
using Database.Abstractions;
using Domain;
using Domain.Messages;
using MongoDB.Driver;
using Utility;

namespace Write.Application.UseCases;

public class SaveURLHandler : IUseCaseHandler<Message.Request.CreateLink, Message.Response.LinkCreated>
{
    private readonly IRepositoryBase<LinkDocument> _repository;
    private readonly ICacheService _cacheService;
    public SaveURLHandler(IRepositoryBase<LinkDocument> repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<Message.Response.LinkCreated> HandleAsync(Message.Request.CreateLink input)
    {
        if (!UrlHelper.IsValidUrl(input.OriginalUrl))
            throw new ArgumentException("Invalid URL");

        for (int i = 0; i < UrlHelper.MAX_RETRY; i++)
        {
            var shortCode = UrlHelper.Generate();
            try
            {
                await _repository.InsertOneAsync(new LinkDocument
                {
                    Id = Guid.NewGuid().ToString(),
                    ShortCode = shortCode,
                    OriginalUrl = input.OriginalUrl,
                    CreatedOnUtc = DateTimeOffset.UtcNow
                });

                await _cacheService.SetAsync(
                    shortCode,
                    input.OriginalUrl);

                return new Message.Response.LinkCreated(shortCode);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                // ShortCode bị trùng -> sinh lại và thử tiếp
            }
        }

        throw new InvalidOperationException("Unable to generate a unique short code.");
    }
}
