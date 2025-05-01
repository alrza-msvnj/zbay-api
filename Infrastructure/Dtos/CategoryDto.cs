namespace Infrastructure.Dtos;

public class CategoryDto
{
    public class CategoryCreateDto
    {
        public required string Title { get; set; }
        public int? ParentId { get; set; }
    }
}
