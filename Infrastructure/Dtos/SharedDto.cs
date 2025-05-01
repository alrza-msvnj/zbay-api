namespace Infrastructure.Dtos;

public class SharedDto
{
    public class GetAllDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public required string SearchTerm { get; set; }
        public List<int>? CategoryIds { get; set; }
    }
}
