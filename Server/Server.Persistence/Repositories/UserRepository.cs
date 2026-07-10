using Server.Application.Interfaces.Repository;
using Server.Domain.Entities;
using Server.Persistence.Context;

namespace Server.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>, IUserRepository
{
    public async Task Signup(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}