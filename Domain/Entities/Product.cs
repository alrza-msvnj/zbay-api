using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Product
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string? IgId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public byte DiscountPercentage { get; set; }
    public long Stock { get; set; }
    public string? IgCode { get; set; }
    public string? IgThumbnailSrc { get; set; }
    public string? IgDisplayUrl { get; set; }
    public long? IgLikeCount { get; set; }
    public long? IgCommentCount { get; set; }
    public byte? IgCarouselMediaCount { get; set; }
    public string? IgVideoUrl { get; set; }
    public long ShopId { get; set; }
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

    [JsonIgnore]
    public virtual ICollection<ProductIgCarouselMedia> ProductIgCarouselMedia { get; set; }
    [JsonIgnore]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    [JsonIgnore]
    public virtual Shop Shop { get; set; }
}

public class Dimensions
{
    public int? Hieght { get; set; }
    public int? Width { get; set; }
}

public class Caption
{
    public string? Id { get; set; }
    public string? Text { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class Location
{
    public string? Id { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public string? Name { get; set; }
    public string? AddressJson { get; set; }
}
