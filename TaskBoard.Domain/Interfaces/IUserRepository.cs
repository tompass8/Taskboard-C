using TaskBoard.Domain.Entities;

namespace TaskBoard.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> ExistsUserByEmailAsync(string email);
    Task<User> CreateUserAsync(User user);
}