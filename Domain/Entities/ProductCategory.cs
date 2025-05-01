using System.Text.Json.Serialization;

namespace Domain.Entities;

public class ProductCategory
{
    public long ProductId { get; set; }
    public int CategoryId { get; set; }

    [JsonIgnore]
    public virtual Product Product { get; set; }
    [JsonIgnore]
    public virtual Category Category { get; set; }
}
