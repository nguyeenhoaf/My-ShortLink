namespace Application;

public interface IUseCaseHandler<TInput, TOutput>
{
    public Task<TOutput> HandleAsync(TInput input);
}
