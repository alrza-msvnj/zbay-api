using Domain.Entities;

namespace Infrastructure.Dtos;

public class InstagramDto
{
    public class InstagramPostDto
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? ThumbnailSrc { get; set; }
        public string? DisplayUrl { get; set; }
        public Dimensions? Dimensions { get; set; }
        public long? LikeCount { get; set; }
        public long? CommentCount { get; set; }
        public byte? CarouselMediaCount { get; set; }
        public string? VideoUrl { get; set; }
        public bool? IsVideo { get; set; }
        public Caption? Caption { get; set; }
        public Location? Location { get; set; }
        public List<Media>? CarouselMedia { get; set; }
        public Owner? Owner { get; set; }
    }

    public class Owner
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public long? Followers { get; set; }
        public bool? IsVerified { get; set; }
    }

    public class Media
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? DisplayUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public bool? IsVideo { get; set; }
        public Dimensions? Dimensions { get; set; }
    }
}
