using Domain.Entities;

namespace Infrastructure.Dtos;

public class ProductDto
{
    public class ProductResponseDto
    {
        public ulong Id { get; set; }
        public Guid Uuid { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public byte DiscountPercentage { get; set; }
        public uint Stock { get; set; }
        public byte Rating { get; set; }
        public uint Reviews { get; set; }
        public uint ShopId { get; set; }
        public bool HasDiscount { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public required List<string> Images { get; set; }
        public List<Category>? Categories { get; set; }
        public Shop? Shop { get; set; }
    }

    public class ProductCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal OriginalPrice { get; set; }
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
