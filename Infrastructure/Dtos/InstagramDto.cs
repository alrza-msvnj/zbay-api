namespace Infrastructure.Dtos;

public class InstagramDto
{
    public class InstagramScrapePostsDto
    {
        public required List<string> InstagramUsernames { get; set; }
    }

    public class InstagramPostDto
    {
        public string? Id { get; set; }
        public string? ShortCode { get; set; }
        public string? ThumbnailSrc { get; set; }
        public string? DisplayUrl { get; set; }
        public Dimensions? Dimensions { get; set; }
        public uint? LikeCount { get; set; }
        public uint? CommentCount { get; set; }
        public byte? CarouselMediaCount { get; set; }
        public string? VideoUrl { get; set; }
        public bool? IsVideo { get; set; }
        public Caption? Caption { get; set; }
        public Location? Location { get; set; }
        public List<Media>? CarouselMedia { get; set; }
        public Owner? Owner { get; set; }
    }

    public class Dimensions
    {
        public ushort? Hieght { get; set; }
        public ushort? Width { get; set; }
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

    public class Media
    {
        public string? Id { get; set; }
        public string? ShortCode { get; set; }
        public string? DisplayUrl { get; set; }
        public Dimensions? Dimensions { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public bool? IsVideo { get; set; }
    }

    public class Owner
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public uint? Followers { get; set; }
        public bool? IsVerified { get; set; }
    }
}
