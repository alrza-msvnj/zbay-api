using Newtonsoft.Json;
using static Infrastructure.Dtos.InstagramDto;

namespace Api.Factories;

public static class InstagramFactory
{
    public static InstagramPostDto MapToInstagramPostDto(string data)
    {
        var parsedData = JsonConvert.DeserializeObject<dynamic>(data);

        var instagramPost = new InstagramPostDto
        {
            Id = parsedData["data"]?["xdt_shortcode_media"]?["id"]?.ToString(),
            ShortCode = parsedData["data"]?["xdt_shortcode_media"]?["shortcode"]?.ToString(),
            ThumbnailSrc = parsedData["data"]?["xdt_shortcode_media"]?["thumbnail_src"]?.ToString(),
            DisplayUrl = parsedData["data"]?["xdt_shortcode_media"]?["display_url"]?.ToString(),
            Dimensions = new Dimensions
            {
                Hieght = int.Parse(parsedData["data"]?["xdt_shortcode_media"]?["dimensions"]?["height"]?.ToString()),
                Width = int.Parse(parsedData["data"]?["xdt_shortcode_media"]?["dimensions"]?["width"]?.ToString())
            },
            IsVideo = bool.Parse(parsedData["data"]?["xdt_shortcode_media"]?["is_video"]?.ToString()),
            LikeCount = int.Parse(parsedData["data"]?["xdt_shortcode_media"]?["edge_media_preview_like"]?["count"]?.ToString()),
            CommentCount = int.Parse(parsedData["data"]?["xdt_shortcode_media"]?["edge_media_to_parent_comment"]?["count"]?.ToString()),
            Owner = new Owner
            {
                Id = parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["id"]?.ToString(),
                Username = parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["username"]?.ToString(),
                FullName = parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["full_name"]?.ToString(),
                ProfilePictureUrl = parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["profile_pic_url"]?.ToString(),
                Followers = int.Parse(parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["edge_followed_by"]?["count"]?.ToString()),
                IsVerified = bool.Parse(parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["is_verified"]?.ToString())
            },
            Caption = new Caption
            {
                Id = parsedData["data"]?["xdt_shortcode_media"]?["edge_media_to_caption"]?["edges"]?[0]?["node"]?["id"]?.ToString(),
                Text = parsedData["data"]?["xdt_shortcode_media"]?["edge_media_to_caption"]?["edges"]?[0]?["node"]?["text"]?.ToString(),
                CreatedDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(parsedData["data"]?["xdt_shortcode_media"]?["edge_media_to_caption"]?["edges"]?[0]?["node"]?["created_at"]?.ToString())).DateTime
            },
            Slides = new List<Slide>()
        };

        var slides = parsedData["data"]?["xdt_shortcode_media"]?["edge_sidecar_to_children"]?["edges"].ToObject<List<object>>();
        foreach (var slide in slides)
        {
            instagramPost.Slides.Add(new Slide
            {
                Id = slide["node"]["id"],
                ShortCode = slide["node"]["shortcode"],
                DisplayUrl = slide["node"]["display_url"],
                Dimensions = new Dimensions
                {
                    Hieght = slide["node"]["dimensions"]["height"],
                    Width = slide["node"]["dimensions"]["width"]
                },
                IsVideo = bool.Parse(slide["node"]["is_video"].ToString())
            });
        }

        return instagramPost;
    }
}
