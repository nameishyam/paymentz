using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Users.Commands.Signup;

namespace Server.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupRequest request)
    {
        var id = await mediator.Send(new SignupCommand(request));

        return CreatedAtAction(
            nameof(Signup),
            new { id },
            new
            {
                Id = id
            });
    }
}