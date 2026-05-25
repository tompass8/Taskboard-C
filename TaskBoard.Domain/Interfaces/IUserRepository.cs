using TaskBoard.Domain.Entities;

namespace TaskBoard.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<bool> ExistsUserByEmailAsync(string email);
    Task<User> CreateUserAsync(User user);
}