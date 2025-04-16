namespace Infrastructure.Dtos;

public class InstagramDto
{
    public class InstagramScrapePostsDto
    {
        public required List<string> InstagramUsernames { get; set; }
    }

    public class InstagramPostDto
    {
        public string Id { get; set; }
        public string ShortCode { get; set; }
        public string ThumbnailSrc { get; set; }
        public string DisplayUrl { get; set; }
        public Dimensions Dimensions { get; set; }
        // TODO - convert to ushort
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsVideo { get; set; }
        public Owner Owner { get; set; }
        public Caption Caption { get; set; }
        public List<Slide> Slides { get; set; }
    }

    public class Dimensions
    {
        // TODO - convert to ushort
        public int Hieght { get; set; }
        public int Width { get; set; }
    }

    public class Owner
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        // TODO - convert to ushort
        public int Followers { get; set; }
        public bool IsVerified { get; set; }
    }

    public class Slide
    {
        public string Id { get; set; }
        public string ShortCode { get; set; }
        public string DisplayUrl { get; set; }
        public Dimensions Dimensions { get; set; }
        public bool IsVideo { get; set; }
    }

    public class Caption
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAtUnix { get; set; }
    }
}
