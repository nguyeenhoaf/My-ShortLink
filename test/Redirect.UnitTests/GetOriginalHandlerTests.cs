using Caching;
using Database.Abstractions;
using Domain;
using Domain.Messages;
using Moq;
using Redirect.Application.UseCases;
using System.Linq.Expressions;

namespace Redirect.UnitTests;

public class GetOriginalHandlerTests
{
    private readonly Mock<IRepositoryBase<LinkDocument>> _mockRepo;
    private readonly Mock<ICacheService> _mockCache;
    private readonly GetOriginalHandler _handler;
    public GetOriginalHandlerTests()
    {
        _mockRepo = new Mock<IRepositoryBase<LinkDocument>>();
        _mockCache = new Mock<ICacheService>();
        _handler = new GetOriginalHandler(_mockRepo.Object, _mockCache.Object);
    }

    [Fact]
    public async Task Handle_WhenCacheHit_ShouldReturnOriginalUrl()
    {
        string shortCode = "aB3xY9kL";
        var command = new Message.Request.GetOriginalUrl(shortCode);
        _mockCache
            .Setup(x => x.GetAsync<string>(shortCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://google.com");

        var result = await _handler.HandleAsync(command);
        Assert.Equal("https://google.com", result.OriginalUrl);
        _mockRepo.Verify(
            x => x.FindOneAsync(It.IsAny<Expression<Func<LinkDocument, bool>>>()),
            Times.Never);

        _mockCache.Verify(
            x => x.SetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCacheMiss_ShouldReadDatabase_AndSetCache()
    {
        string shortCode = "aB3xY9kG";
        var command = new Message.Request.GetOriginalUrl(shortCode);
        _mockCache
            .Setup(x => x.GetAsync<string>(shortCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        _mockRepo
            .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<LinkDocument, bool>>>()))
            .ReturnsAsync(new LinkDocument
            {
                ShortCode = shortCode,
                OriginalUrl = "https://google.com"
            });

        var result = await _handler.HandleAsync(command);

        Assert.Equal("https://google.com", result.OriginalUrl);

        _mockRepo.Verify(
            x => x.FindOneAsync(It.IsAny<Expression<Func<LinkDocument, bool>>>()),
            Times.Once);

        _mockCache.Verify(
            x => x.SetAsync(shortCode, "https://google.com", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenShortCodeNotFound_ShouldThrowException()
    {
        string shortCode = "aB3xY9kL";
        var command = new Message.Request.GetOriginalUrl(shortCode);
        _mockCache
            .Setup(x => x.GetAsync<string>(shortCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        _mockRepo
            .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<LinkDocument, bool>>>()))
            .ReturnsAsync((LinkDocument?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.HandleAsync(command));

        _mockCache.Verify(
            x => x.SetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}