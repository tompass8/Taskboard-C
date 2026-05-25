using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Infrastructure.Services;

// BCrypt is confined here

// Infrastructure is the only layer aware of this technical detail.
// Application layer only knows IPasswordHasher interface, not BCrypt itself.
public class PasswordHasher : IPasswordHasher
{
    // ~100ms intentionally — slows down brute force attacks on stolen passwords
    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
    
    // Said to be mathematically impossible to reverse
    public bool Verify(string password, string hash) 
        => BCrypt.Net.BCrypt.Verify(password, hash);
}

