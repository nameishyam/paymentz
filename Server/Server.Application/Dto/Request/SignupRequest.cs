namespace Server.Application.Dto.Request;

public class SignupRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConformPassword { get; set; }
}