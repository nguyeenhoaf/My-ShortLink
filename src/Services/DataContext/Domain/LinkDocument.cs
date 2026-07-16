using Domain.Abstractions;
using Domain.Attributes;
using Domain.Constants;

namespace Domain;

[BsonCollection(nameof(CollectionNames.Link))]
public class LinkDocument : IDocument
{
    public string Id { get; set; }
    public string ShortCode { get; set; }
    public string OriginalUrl { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
}
