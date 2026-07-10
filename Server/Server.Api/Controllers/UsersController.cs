using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Dto.Request;
using Server.Application.Users.Commands;

namespace Server.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupRequest request)
    {
        var response = await mediator.Send(new SignupCommand(request));

        SetCookie(response.AccessToken);

        return CreatedAtAction(
            nameof(Signup),
            new { response.Id },
            new
            {
                Id = response.Id
            });
    }

    private void SetCookie(string accessToken)
    {
        Response.Cookies.Append(
            "access_token",
            accessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
    }
}