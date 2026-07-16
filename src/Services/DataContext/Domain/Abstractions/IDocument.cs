namespace Domain.Abstractions;

public interface IDocument
{
    string Id { get; set; }
    DateTimeOffset CreatedOnUtc { get; }
}