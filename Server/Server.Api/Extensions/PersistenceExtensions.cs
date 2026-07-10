using Microsoft.EntityFrameworkCore;
using Server.Application.Interfaces.Repository;
using Server.Persistence.Context;
using Server.Persistence.Repositories;

namespace Server.Api.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("Database"));
        });

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}