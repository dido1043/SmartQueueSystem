using Auth.Domain.Data.Model;
namespace Auth.Application.Repository;

public interface IUserRepository
{
     Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);

    Task AddAsync(User user, CancellationToken ct = default);
    void Update(User user);
    void Delete(User user);

    Task<int> SaveChangesAsync(CancellationToken ct = default);

}
