using Server.Domain.Entities;

namespace Server.Application.Interfaces.Repository;

public interface IUserRepository
{
    Task Signup(User user);
}