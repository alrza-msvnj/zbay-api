using Domain.Entities;

namespace Infrastructure.Dtos;

public class ProductDto
{
    public class ProductResponseDto
    {
        public ulong Id { get; set; }
        public Guid Uuid { get; set; }
        public string? IgId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public byte DiscountPercentage { get; set; }
        public uint Stock { get; set; }
        public string? IgCode { get; set; }
        public string? IgThumbnailSrc { get; set; }
        public string? IgDisplayUrl { get; set; }
        public uint? IgLikeCount { get; set; }
        public uint? IgCommentCount { get; set; }
        public byte? IgCarouselMediaCount { get; set; }
        public string? IgVideoUrl { get; set; }
        public uint ShopId { get; set; }
        public bool HasDiscount { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IgIsVideo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public required List<string> Images { get; set; }
        public Dimensions? IgDimensions { get; set; }
        public Caption? IgCaption { get; set; }
        public Location? IgLocation { get; set; }
        public List<ProductIgCarouselMedia>? IgCarouselMedia { get; set; }
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
        public required List<string> Images { get; set; }
        public required List<ushort> CategoryIds { get; set; }
    }

    public class ProductCreateIgDto
    {
        public List<string> Usernames { get; set; }
        public List<ushort> CategoryIds { get; set; }
    }

    public class ProductUpdateDto
    {
        // TODO
        public ulong Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public byte? DiscountPercentage { get; set; }
        public uint? Stock { get; set; }
        public List<string>? Images { get; set; }
    }
}
