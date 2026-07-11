using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Dto.Request;
using Server.Application.Exceptions;
using Server.Application.Users.Commands;

namespace Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    private const string CookieName = "access_token";

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> Signup(SignupRequest request)
    {
        try
        {
            var response = await mediator.Send(new SignupCommand(request));

            return CreatedAtAction(
                nameof(Signup),
                new { response.Id },
                new { response.Id });
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var response = await mediator.Send(new LoginCommand(request));

            return Ok(new
            {
                id = response.Id,
                token = response.AccessToken
            });
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidDetailsException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        try
        {
            Response.Cookies.Delete(CookieName);

            return Ok("Logout successful");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}