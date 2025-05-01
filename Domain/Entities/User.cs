using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class User
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Password { get; set; }
    public int? LastOtp { get; set; }
    public long? ShopId { get; set; }
    public UserRole Role { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreateDate { get; set; }

    [JsonIgnore]
    public virtual Shop Shop { get; set; }
}
