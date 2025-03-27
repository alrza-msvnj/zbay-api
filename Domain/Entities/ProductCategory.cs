using System.Text.Json.Serialization;

namespace Domain.Entities;

public class ProductCategory
{
    public ulong ProductId { get; set; }
    public ushort CategoryId { get; set; }

    [JsonIgnore]
    public virtual Product Product { get; set; }
    [JsonIgnore]
    public virtual Category Category { get; set; }
}
