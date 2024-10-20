using Identity.API.Features.Users;
using Identity.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender, UserEmailService userEmailService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] Register.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return BadRequest(result.ErrorTypes);

        return NoContent();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] Login.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return BadRequest(result.ErrorTypes);

        return Ok(result.Value);
    }

    [HttpGet(nameof(ForgotPassword))]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var result = await userEmailService.SendResetPasswordEmail(email);
        if (result.IsFailure)
            return BadRequest(result.ErrorTypes);

        return Ok(result);
    }

    [HttpPost(nameof(ResetPassword))]
    public async Task<IActionResult> ResetPassword(ResetPassword.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return BadRequest(result.ErrorTypes);

        return Ok(result);
    }
}
