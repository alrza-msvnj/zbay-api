namespace Domain.Entities;

public class Shop
{
    public uint Id { get; set; }
    public Guid Uuid { get; set; }
    public string InstagramId { get; set; }
    public string InstagramUrl { get; set; }
    public string Name { get; set; }
    public uint Followers { get; set; }
    public string Logo { get; set; }
    public uint OwnerId { get; set; }
    public bool IsVerified { get; set; }
    public bool IsValidated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime JoinDate { get; set; }
    public List<ushort>? CategoryIds { get; set; }

    public virtual User Owner { get; set; }
    public virtual ICollection<ShopCategory> ShopCategories { get; set; }
}
