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

            SetAuthCookie(response.AccessToken);

            return StatusCode(201, new
            {
                response.Id,
                response.AccessToken
            });
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

            SetAuthCookie(response.AccessToken);

            return Ok(new
            {
                response.Id,
                response.AccessToken
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

            Console.WriteLine("Logout successful");

            return Ok("Logout successful");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var response = await mediator.Send(new RefreshTokenCommand(request));

            SetAuthCookie(response.AccessToken);

            return Ok(new
            {
                response.AccessToken,
                response.RefreshToken
            });
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
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

    private void SetAuthCookie(string token)
    {
        Response.Cookies.Append(
            CookieName,
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
    }
}