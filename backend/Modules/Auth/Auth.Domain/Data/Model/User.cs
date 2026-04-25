using System;
using Auth.Domain.Data.Enum;
using Microsoft.AspNetCore.Identity;
namespace Auth.Domain.Data.Model;

public class User: IdentityUser<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }

    public User() { }
    public User(Guid id, string name, string email, string passwordHash, UserRole role)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}
