using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Shop
{
    public uint Id { get; set; }
    public Guid Uuid { get; set; }
    public required string InstagramId { get; set; }
    public required string InstagramUrl { get; set; }
    public required string Name { get; set; }
    public uint Followers { get; set; }
    public required string Logo { get; set; }
    public uint OwnerId { get; set; }
    public bool IsVerified { get; set; }
    public bool IsValidated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime JoinDate { get; set; }
    public List<ushort>? CategoryIds { get; set; }

    [JsonIgnore]
    public virtual User Owner { get; set; }
    [JsonIgnore]
    public virtual ICollection<ShopCategory> ShopCategories { get; set; }
}
