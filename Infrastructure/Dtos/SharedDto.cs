namespace Infrastructure.Dtos;

public class SharedDto
{
    public class GetAllDto
    {
        public ushort PageNumber { get; set; }
        public ushort PageSize { get; set; }
        public required string SearchTerm { get; set; }
        public List<ushort>? CategoryIds { get; set; }
    }
}
