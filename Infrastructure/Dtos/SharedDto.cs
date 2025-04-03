namespace Infrastructure.Dtos;

public class SharedDto
{
    public class GetAllDto
    {
        public List<ushort>? CategoryIds { get; set; }
        public ushort PageNumber { get; set; }
        public ushort PageSize { get; set; }
    }
}
