using System.Linq;
using Auth.Application.Repository;
using Auth.Domain.Data.Model;
using Auth.Domain.Data;
using Microsoft.EntityFrameworkCore;
namespace Auth.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        return _context.Users.AnyAsync(u => u.Email == email, ct);
    }

    public Task AddAsync(User user, CancellationToken ct = default)
    {
       return _context.Users.AddAsync(user, ct).AsTask();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }
}
