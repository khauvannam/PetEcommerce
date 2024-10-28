using Identity.API.Features.Tokens;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController(ISender sender) : ControllerBase
{
    [HttpGet("Refresh")]
    public async Task<IActionResult> Refresh([FromQuery] string accessToken)
    {
        var result = await sender.Send(new Refresh.Command(accessToken));
        if (result.IsFailure)
            return BadRequest(result.ErrorTypes);
        return Ok(result);
    }

    [HttpGet("Revoke")]
    public async Task<IActionResult> Revoke(Revoke.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return BadRequest(result.ErrorTypes);
        return Ok(result);
    }
}
