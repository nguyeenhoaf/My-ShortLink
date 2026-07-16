using Application;
using Caching;
using Database.Abstractions;
using Domain;
using Domain.Messages;

namespace Redirect.Application.UseCases;

public class GetOriginalHandler : IUseCaseHandler<Message.Request.GetOriginalUrl, Message.Response.OriginalUrlGot>
{
    private readonly IRepositoryBase<LinkDocument> _repository;
    private readonly ICacheService _cacheService;
    public GetOriginalHandler(IRepositoryBase<LinkDocument> repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<Message.Response.OriginalUrlGot> HandleAsync(Message.Request.GetOriginalUrl input)
    {
        var originalUrl = await _cacheService.GetAsync<string>(input.ShortCode);
        if (!string.IsNullOrEmpty(originalUrl))
            return new Message.Response.OriginalUrlGot(originalUrl);

        var link = await _repository.FindOneAsync(x => x.ShortCode == input.ShortCode);
        if (link is null)
            throw new KeyNotFoundException("Short code not found.");

        await _cacheService.SetAsync(
            input.ShortCode,
            link.OriginalUrl);

        return new Message.Response.OriginalUrlGot(link.OriginalUrl);
    }
}
