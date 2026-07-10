namespace Server.Application.Dto.Response;

public class LoginResponse
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; }
}