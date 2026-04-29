using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Booking.Domain.Data.Enum;
using BusinessEntity = Business.Domain.Data.Model.Business;
using UserEntity = Auth.Domain.Data.Model.User;
namespace Booking.Domain.Data.Model;

[Table("Bookings")]
public class Booking
{
    [Key]
    public Guid Id { get; private set; }
    [Required]
    [ForeignKey("Business")]
    public Guid BusinessId { get; private set; }
    [Required]
    [MaxLength(100)]
    public string Service { get; private set; }
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; private set; }
    [Required]
    public DateTime StartTime { get; private set; }
    [Required]
    public DateTime EndTime { get; private set; }
    public BookingStatus Status { get; private set; }

    public Booking(Guid businessId, string service, Guid userId, DateTime startTime, DateTime endTime, BookingStatus status)
    {
        Id = Guid.NewGuid();
        BusinessId = businessId;
        Service = service;
        UserId = userId;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
    }

}
