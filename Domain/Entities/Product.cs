using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Product
{
    public ulong Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public byte DiscountPercentage { get; set; }
    public uint Stock { get; set; }
    public uint ShopId { get; set; }
    public bool HasDiscount { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public List<byte[]> Images { get; set; }

    [JsonIgnore]
    public virtual Shop Shop { get; set; }
    [JsonIgnore]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; }
}
