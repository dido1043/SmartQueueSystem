using System;
using Auth.Domain.Data.Enum;
using Microsoft.AspNetCore.Identity;
namespace Auth.Domain.Data.Model;

public class User: IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
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
