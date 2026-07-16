namespace Domain.Messages;

public static class Message
{
    public static class Request
    {
        public record CreateLink(string OriginalUrl);
        public record GetOriginalUrl(string ShortCode);
    }

    public static class Response 
    { 
        public record LinkCreated(string ShortCode);
        public record OriginalUrlGot(string OriginalUrl);
    }
}
