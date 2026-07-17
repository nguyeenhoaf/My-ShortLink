using Caching;
using Database.Abstractions;
using Domain;
using Domain.Messages;
using Moq;
using Write.Application.UseCases;

namespace Write.UnitTests;

public class SaveURLHandlerTests
{
    private readonly Mock<IRepositoryBase<LinkDocument>> _mockRepo;
    private readonly Mock<ICacheService> _mockCache;
    private readonly SaveURLHandler _handler;

    public SaveURLHandlerTests()
    {
        _mockRepo = new Mock<IRepositoryBase<LinkDocument>>();
        _mockCache = new Mock<ICacheService>();
        _handler = new SaveURLHandler(_mockRepo.Object, _mockCache.Object);
    }

    [Fact]
    public async Task Handle_WhenValidUrl_ShouldSaveAndReturnShortCode()
    {
        var request = new Message.Request.CreateLink("https://google.com");
        _mockRepo
            .Setup(x => x.InsertOneAsync(It.IsAny<LinkDocument>()))
            .Returns(Task.CompletedTask);

        _mockCache
            .Setup(x => x.SetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.HandleAsync(request);
        Assert.False(string.IsNullOrWhiteSpace(result.ShortCode));

        _mockRepo.Verify(
            x => x.InsertOneAsync(It.IsAny<LinkDocument>()),
            Times.Once);

        _mockCache.Verify(
            x => x.SetAsync(
                result.ShortCode,
                "https://google.com",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUrlInvalid_ShouldThrowArgumentException()
    {
        var request = new Message.Request.CreateLink("abcxyz");
        await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.HandleAsync(request));
        _mockRepo.Verify(
            x => x.InsertOneAsync(It.IsAny<LinkDocument>()),
            Times.Never);
    }

    //[Fact]
    //public async Task Handle_WhenDuplicateKey_ShouldRetryUntilSuccess()
    //{
    //    var request = new Message.Request.CreateLink("https://google.com");
    //    var duplicateException = CreateDuplicateKeyException();
    //    _mockRepo
    //        .SetupSequence(x => x.InsertOneAsync(It.IsAny<LinkDocument>()))
    //        .ThrowsAsync(duplicateException)
    //        .ThrowsAsync(duplicateException)
    //        .Returns(Task.CompletedTask);
    //    _mockCache
    //        .Setup(x => x.SetAsync(
    //            It.IsAny<string>(),
    //            It.IsAny<string>(),
    //            It.IsAny<CancellationToken>()))
    //        .Returns(Task.CompletedTask);

    //    var result = await _handler.HandleAsync(request);
    //    Assert.NotNull(result);
    //    _mockRepo.Verify(
    //        x => x.InsertOneAsync(It.IsAny<LinkDocument>()),
    //        Times.Exactly(3));
    //}
}