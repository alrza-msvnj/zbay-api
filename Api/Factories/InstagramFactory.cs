using Newtonsoft.Json;
using static Infrastructure.Dtos.InstagramDto;

namespace Api.Factories;

public static class InstagramFactory
{
    public static InstagramPostDto MapToInstagramPostDto(string data)
    {
        var parsedData = JsonConvert.DeserializeObject(data);

        var instagramPost = new InstagramPostDto
        {
            Id = parsedData["id"].ToString(),
            ShortCode = parsedData["shortcode"].ToString(),
            ThumbnailSrc = parsedData["thumbnail_src"].ToString(),
            DisplayUrl = parsedData["display_url"].ToString(),
            Dimensions = new Dimensions
            {
                Hieght = int.Parse(parsedData["dimensions"]["height"].ToString()),
                Width = int.Parse(parsedData["dimensions"]["width"].ToString())
            },
            LikeCount = int.Parse(parsedData["like_count"].ToString()),
            CommentCount = int.Parse(parsedData["comment_count"].ToString()),
            IsVideo = bool.Parse(parsedData["is_video"].ToString()),
            Owner = new Owner
            {
                Id = parsedData["owner"]["id"].ToString(),
                Username = parsedData["owner"]["username"].ToString(),
                FullName = parsedData["owner"]["full_name"].ToString(),
                ProfilePictureUrl = parsedData["owner"]["profile_picture_url"].ToString(),
                Followers = int.Parse(parsedData["owner"]["followers_count"].ToString()),
                IsVerified = bool.Parse(parsedData["owner"]["is_verified"].ToString())
            },
            Caption = new Caption
            {
                Id = parsedData["caption"]["id"].ToString(),
                Text = parsedData["caption"]["text"].ToString(),
                CreatedAtUnix = DateTimeOffset.FromUnixTimeSeconds(long.Parse(parsedData["caption"]["created_at_unix"].ToString())).DateTime
            },
            Slides = new List<Slide>()
        };
    }
}
