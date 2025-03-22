using System.Text.Json.Serialization;

namespace Domain.Entities;

public class ShopCategory
{
    public uint ShopId { get; set; }
    public ushort CategoryId { get; set; }

    [JsonIgnore]
    public virtual Shop Shop { get; set; }
    [JsonIgnore]
    public virtual Category Category { get; set; }
}
