using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Interfaces;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
        => await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> ExistsUserByEmailAsync(string email)
        => await _context.Users
            .AnyAsync(u => u.Email == email);

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}