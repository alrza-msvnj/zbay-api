namespace Infrastructure.Dtos;

public class CategoryDto
{
    public class CategoryCreateDto
    {
        public required string Title { get; set; }
        public ushort? ParentId { get; set; }
    }
}
