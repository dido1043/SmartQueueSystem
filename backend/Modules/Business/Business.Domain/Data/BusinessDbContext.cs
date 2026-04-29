using System;
using Microsoft.EntityFrameworkCore;
using Auth.Domain.Data.Model;
using BusinessEntity = Business.Domain.Data.Model.Business;
namespace Business.Domain.Data;

public class BusinessDbContext : DbContext
{
    public BusinessDbContext()
    {
    }

    public BusinessDbContext(DbContextOptions<BusinessDbContext> options) : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BusinessEntity>()
            .ToTable("Business")
            .HasKey(b => b.Id);

        modelBuilder.Entity<User>().ToTable("AspNetUsers", t => t.ExcludeFromMigrations());

        modelBuilder.Entity<BusinessEntity>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
            
    }
}
