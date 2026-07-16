using Application;
using Domain.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Write.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WriteController : ControllerBase
{
    private readonly ILogger<WriteController> _logger;
    private readonly IUseCaseHandler<Message.Request.CreateLink, Message.Response.LinkCreated> _useCase;

    public WriteController(
        ILogger<WriteController> logger, 
        IUseCaseHandler<Message.Request.CreateLink, Message.Response.LinkCreated> useCase)
    {
        _logger = logger;
        _useCase = useCase;
    }

    [HttpPost]
    public async Task<IResult> Create([FromBody] Message.Request.CreateLink command)
    {
        try
        {
            var result = await _useCase.HandleAsync(command);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating link.");
            return Results.Problem(ex.Message);
        }
    }
}