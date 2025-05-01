using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Shop
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string? IgId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
    public int TotalProducts { get; set; }
    public string? IgUsername { get; set; }
    public string? IgFullName { get; set; }
    public long? IgFollowers { get; set; }
    public long? OwnerId { get; set; }
    public bool IsVerified { get; set; }
    public bool IsValidated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime JoinDate { get; set; }

    [JsonIgnore]
    public virtual User Owner { get; set; }
    [JsonIgnore]
    public virtual ICollection<ShopCategory> ShopCategories { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; }
}
