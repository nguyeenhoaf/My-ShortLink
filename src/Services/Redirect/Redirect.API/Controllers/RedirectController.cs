using Application;
using Domain.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Redirect.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedirectController : ControllerBase
    {

        private readonly ILogger<RedirectController> _logger;
        private readonly IUseCaseHandler<Message.Request.GetOriginalUrl, Message.Response.OriginalUrlGot> _useCase;
        public RedirectController(ILogger<RedirectController> logger, IUseCaseHandler<Message.Request.GetOriginalUrl, Message.Response.OriginalUrlGot> useCase)
        {
            _logger = logger;
            _useCase = useCase;
        }

        [HttpGet("{shortCode}")]
        public async Task<IResult> GetOriginalUrl(string shortCode)
        {
            try
            {
                var result = await _useCase.HandleAsync(new Message.Request.GetOriginalUrl(shortCode));
                return Results.Ok(result.OriginalUrl);
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error occurred while retrieving original URL.");
                return Results.Problem(ex.Message);
            }
        }
    }
}
