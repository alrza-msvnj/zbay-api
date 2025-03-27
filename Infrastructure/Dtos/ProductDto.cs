namespace Infrastructure.Dtos;

public class ProductDto
{
    public class ProductCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public byte DiscountPercentage { get; set; }
        public uint Stock { get; set; }
        public uint ShopId { get; set; }
        public bool IsAvailable { get; set; }
        public required List<string> Images { get; set; }
        public required List<ushort> CategoryIds { get; set; }
    }

    public class ProductUpdateDto
    {
        public ulong Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public byte? DiscountPercentage { get; set; }
        public uint? Stock { get; set; }
        public bool? IsAvailable { get; set; }
        public List<string>? Images { get; set; }
    }
}
