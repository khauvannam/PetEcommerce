using Identity.API.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] Register.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorTypes);
        }
        return Ok("User is created successfully");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(Login.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorTypes);
        }

        return Ok(result);
    }

    [HttpGet(nameof(ForgotPassword))]
    public async Task<IActionResult> ForgotPassword(ForgotPassword.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorTypes);
        }

        return Ok(result);
    }

    [HttpPost(nameof(ResetPassword))]
    public async Task<IActionResult> ResetPassword(ResetPassword.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorTypes);
        }
        return Ok(result);
    }
}
