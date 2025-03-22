namespace Domain.Entities;

public class Category
{
    public ushort Id { get; set; }
    public string Title { get; set; }
    public ushort? ParentId { get; set; }

    public virtual Category Parent { get; set; }
    public virtual ICollection<Category> Children { get; set; }
    public virtual ICollection<ShopCategory> ShopCategories { get; set; }
}
