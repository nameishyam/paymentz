using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Application.Configurations;
using Server.Application.Interfaces.Repository;
using Server.Domain.Entities;
using Server.Persistence.Context;

namespace Server.Persistence.Repositories;

public class UserRepository(
    ApplicationDbContext context,
    IOptions<ConnectionStrings> connectionStrings) 
    : BaseRepository<User>(connectionStrings.Value.Database), 
        IUserRepository
{
    public async Task<bool> ExistsByEmail(string email)
    {
        return await context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<User> GetByEmail(string email)
    {
        return await context.Users
            .FirstAsync(u => u.Email == email);
    }

    public async Task Create(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}