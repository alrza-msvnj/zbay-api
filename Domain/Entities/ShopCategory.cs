namespace Domain.Entities;

public class ShopCategory
{
    public uint ShopId { get; set; }
    public ushort CategoryId { get; set; }

    public virtual Shop Shop { get; set; }
    public virtual Category Category { get; set; }
}
