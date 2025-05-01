using System.Text.Json.Serialization;

namespace Domain.Entities;

public class ShopCategory
{
    public long ShopId { get; set; }
    public int CategoryId { get; set; }

    [JsonIgnore]
    public virtual Shop Shop { get; set; }
    [JsonIgnore]
    public virtual Category Category { get; set; }
}
