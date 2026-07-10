using Server.Domain.Entities;

namespace Server.Application.Interfaces.Repository;

public interface IUserRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<User> GetByEmail(string email);
    Task Create(User user);
}