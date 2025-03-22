using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Category
{
    public ushort Id { get; set; }
    public required string Title { get; set; }
    public ushort? ParentId { get; set; }

    [JsonIgnore]
    public virtual Category Parent { get; set; }
    [JsonIgnore]
    public virtual ICollection<Category> Children { get; set; }
    [JsonIgnore]
    public virtual ICollection<ShopCategory> ShopCategories { get; set; }
}
