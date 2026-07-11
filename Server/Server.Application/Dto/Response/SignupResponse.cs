namespace Server.Application.Dto.Response;

public class SignupResponse
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}