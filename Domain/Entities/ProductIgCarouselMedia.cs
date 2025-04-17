using System.Text.Json.Serialization;

namespace Domain.Entities;

public class ProductIgCarouselMedia
{
    public ulong Id { get; set; }
    public Guid Uuid { get; set; }
    public string? IgId { get; set; }
    public string? ShortCode { get; set; }
    public string? DisplayUrl { get; set; }
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public byte Order { get; set; }
    public ulong ProductId { get; set; }
    public bool? IsVideo { get; set; }
    public Dimensions? Dimensions { get; set; }

    [JsonIgnore]
    public virtual Product Product { get; set; }
}
