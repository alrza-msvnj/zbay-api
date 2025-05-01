using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int? ParentId { get; set; }

    [JsonIgnore]
    public virtual Category Parent { get; set; }
    [JsonIgnore]
    public virtual ICollection<Category> Children { get; set; }
    [JsonIgnore]
    public virtual ICollection<ShopCategory> ShopCategories { get; set; }
    [JsonIgnore]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; }
}
