namespace Domain.Entities;

public class Category
{
    public ushort Id { get; set; }
    public string Title { get; set; }
    public ushort ParentId { get; set; }
}
