namespace Domain.Entities;

public class User
{
    public uint Id { get; set; }
    public Guid Uuid { get; set; }
    public string Username { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string Password { get; set; }
    public ushort? ShopId { get; set; }
    public bool IsShopOwner { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreateDate { get; set; }
}
