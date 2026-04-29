using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserEntity = Auth.Domain.Data.Model.User;
namespace Business.Domain.Data.Model;

public class Business
{
    [Key]
    public Guid Id { get; private set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; private set; }
    [ForeignKey("User")]
    public Guid OwnerId { get; private set; }
    
    [Required]
    public DateTime CreatedAt { get; private set; }

    public Business(string name, Guid ownerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        OwnerId = ownerId;
        CreatedAt = DateTime.UtcNow;
    }
}
