using AuthService.Common.Result;
using AuthService.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers.Base;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        var response = new ApiResult<T>
        {
            IsSuccess = result.IsSuccess,
            Error = result.Error,
            TraceId = result.TraceId,
            ExecutionTimeMs = result.ExecutionTime.TotalMilliseconds,
            Data = result.IsSuccess ? result.Value : default
        };

        if (result.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }

    protected IActionResult HandleResult(Common.Result.Base.Result result)
    {
        var response = new ApiResult
        {
            IsSuccess = result.IsSuccess,
            Error = result.Error,
            TraceId = result.TraceId,
            ExecutionTimeMs = result.ExecutionTime.TotalMilliseconds
        };

        if (result.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
}