using Server.Domain.Entities;

namespace Server.Application.Interfaces.Repository;

public interface IUserRepository
{
    Task<bool> ExistsByEmail(string email);
    Task<bool> ExistsById(Guid userId);
    Task<User> GetByEmail(string email);
    Task<User> GetById(Guid userId);
    Task Create(User user);
    Task Update(User user);
}