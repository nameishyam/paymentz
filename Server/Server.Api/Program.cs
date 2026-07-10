using Server.Api.Extensions;

namespace Server.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services
            .AddApplication()
            .AddPersistence(builder.Configuration);

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}