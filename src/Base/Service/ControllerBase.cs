using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Types.Types.Option;

namespace Service;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected IActionResult OptionResult<T>(Option<T> option)
    {
        if (option.Ok)
        {
            return Ok(option.Value);
        }

        switch (option.Error.Type)
        {
            case ErrorType.NotFound:
                return NotFound(option.Error.GetErrorWrapper());
            case ErrorType.AlreadyExists:
                return Conflict(option.Error.GetErrorWrapper());
            case ErrorType.BadRequest:
            case ErrorType.ValidationError:
                return BadRequest(option.Error.GetErrorWrapper());
            case ErrorType.ServiceError:
            case ErrorType.Unknown:
#if DEBUG
                return StatusCode(500, option.Error.GetErrorWrapper());
#else
                return StatusCode(500);
#endif
            default:
                throw new UnreachableException("CreateActionResult reached unreachable code.");
        }
    }
}