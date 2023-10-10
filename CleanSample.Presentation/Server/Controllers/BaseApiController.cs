using CleanSample.Utility;

namespace CleanSample.Presentation.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected readonly ILogger<BaseApiController> Logger;
    protected readonly IMediator Mediator;

    public BaseApiController(ILogger<BaseApiController> logger, IMediator mediator)
    {
        Logger = logger;
        Mediator = mediator;
    }

    protected ActionResult ExtractResult<T>(Result<T> result) where T : class
    {
        return result.IsSuccess ? Ok(result.Value) : StatusCode(result.Error.Code, result.Error.Message) ;
    }

    protected ActionResult ExtractResult(Result result)
    {
        return result.IsSuccess ? NoContent() : StatusCode(result.Error.Code, result.Error.Message) ;
    }
}