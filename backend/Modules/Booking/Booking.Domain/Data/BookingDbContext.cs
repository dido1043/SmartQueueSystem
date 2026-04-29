using System;
using Microsoft.EntityFrameworkCore;
using Auth.Domain.Data.Model;
using BusinessEntity = Business.Domain.Data.Model.Business;

using BookingEntity = global::Booking.Domain.Data.Model.Booking;

namespace Booking.Domain.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {
    }

    public DbSet<BookingEntity> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BookingEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.BusinessId).IsRequired();

            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().ToTable("AspNetUsers", t => t.ExcludeFromMigrations());

            b.HasOne<BusinessEntity>()
                .WithMany()
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BusinessEntity>().ToTable("Business", t => t.ExcludeFromMigrations());
        });
    }
}
