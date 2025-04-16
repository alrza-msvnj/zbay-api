using Domain.Entities;
using Newtonsoft.Json;
using static Infrastructure.Dtos.InstagramDto;

namespace Api.Factories;

public static class InstagramFactory
{
    public static InstagramPostDto MapInstagramPostToInstagramPostDto(string data)
    {
        var parsedData = JsonConvert.DeserializeObject<dynamic>(data);

        var instagramPost = new InstagramPostDto
        {
            Id = parsedData["data"]?["xdt_shortcode_media"]?["id"]?.ToString(),
            Code = parsedData["data"]?["xdt_shortcode_media"]?["shortcode"]?.ToString(),
            ThumbnailSrc = parsedData["data"]?["xdt_shortcode_media"]?["thumbnail_src"]?.ToString(),
            DisplayUrl = parsedData["data"]?["xdt_shortcode_media"]?["display_url"]?.ToString(),
            Dimensions = new Dimensions
            {
                Hieght = Convert.ToUInt16(parsedData["data"]?["xdt_shortcode_media"]?["dimensions"]?["height"]?.ToString()),
                Width = Convert.ToUInt16(parsedData["data"]?["xdt_shortcode_media"]?["dimensions"]?["width"]?.ToString())
            },
            LikeCount = Convert.ToUInt32(parsedData["data"]?["xdt_shortcode_media"]?["edge_media_preview_like"]?["count"]?.ToString()),
            CommentCount = Convert.ToUInt32(parsedData["data"]?["xdt_shortcode_media"]?["edge_media_to_parent_comment"]?["count"]?.ToString()),
            IsVideo = bool.Parse(parsedData["data"]?["xdt_shortcode_media"]?["is_video"]?.ToString()),
            VideoUrl = parsedData["data"]?["xdt_shortcode_media"]["video_url"]?.ToString(),
            Owner = new Owner
            {
                Id = parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["id"]?.ToString(),
                Username = parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["username"]?.ToString(),
                FullName = parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["full_name"]?.ToString(),
                ProfilePictureUrl = parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["profile_pic_url"]?.ToString(),
                Followers = Convert.ToUInt32(parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["edge_followed_by"]?["count"]?.ToString()),
                IsVerified = bool.Parse(parsedData["data"]?["xdt_shortcode_media"]?["owner"]?["is_verified"]?.ToString())
            }
        };

        var captions = parsedData["data"]?["xdt_shortcode_media"]?["edge_media_to_caption"]?["edges"]?.ToObject<List<object>>();
        if (captions is not null && captions.Count > 0)
        {
            instagramPost.Caption = new Caption
            {
                Id = captions[0]?["node"]?["id"]?.ToString(),
                Text = captions[0]?["node"]?["text"]?.ToString(),
                CreatedDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(captions[0]?["node"]?["created_at"]?.ToString())).DateTime
            };
        }

        var location = parsedData["data"]?["xdt_shortcode_media"]?["location"];
        if (location is not null)
        {
            instagramPost.Location = new Location
            {
                Id = location["id"]?.ToString(),
                Name = location["name"]?.ToString(),
                AddressJson = location["address_json"]?.ToString()
            };
        }

        var carouselMedia = parsedData["data"]?["xdt_shortcode_media"]?["edge_sidecar_to_children"]?["edges"]?.ToObject<List<object>>();

        if (carouselMedia is null)
        {
            return instagramPost;
        }

        instagramPost.CarouselMedia = new List<Media>();
        foreach (var media in carouselMedia)
        {
            instagramPost.CarouselMedia.Add(new Media
            {
                Id = media["node"]?["id"]?.ToString(),
                ShortCode = media["node"]?["shortcode"]?.ToString(),
                DisplayUrl = media["node"]?["display_url"]?.ToString(),
                Dimensions = new Dimensions
                {
                    Hieght = Convert.ToUInt16(media["node"]?["dimensions"]?["height"]?.ToString()),
                    Width = Convert.ToUInt16(media["node"]?["dimensions"]?["width"]?.ToString())
                },
                VideoUrl = media["node"]?["video_url"]?.ToString(),
                IsVideo = bool.Parse(media["node"]?["is_video"]?.ToString())
            });
        }

        return instagramPost;
    }

    public static List<InstagramPostDto> MapInstagramPostsToInstagramPostsDto(string data)
    {
        var parsedData = JsonConvert.DeserializeObject<dynamic>(data);
        var instagramPosts = parsedData?["data"]?["xdt_api__v1__feed__user_timeline_graphql_connection"]["edges"]?.ToObject<List<object>>();

        if (instagramPosts is null)
        {
            return null!;
        }

        if (instagramPosts.Count == 0)
        {
            return new List<InstagramPostDto>();
        }

        var instagramPostsDto = new List<InstagramPostDto>();
        foreach (var instagramPost in instagramPosts)
        {
            var instagramPostDto = new InstagramPostDto
            {
                Id = instagramPost["node"]?["pk"]?.ToString(),
                Code = instagramPost["node"]?["code"]?.ToString(),
                DisplayUrl = instagramPost["node"]?["display_uri"]?.ToString(),
                Dimensions = new Dimensions
                {
                    Hieght = Convert.ToUInt16(instagramPost["node"]?["original_height"]?.ToString()),
                    Width = Convert.ToUInt16(instagramPost["node"]?["original_width"]?.ToString())
                },
                LikeCount = Convert.ToUInt32(instagramPost["node"]?["like_count"]?.ToString()),
                CommentCount = Convert.ToUInt32(instagramPost["node"]?["comment_count"]?.ToString()),
                Owner = new Owner
                {
                    Id = instagramPost["node"]?["user"]?["pk"]?.ToString(),
                    Username = instagramPost["node"]?["user"]?["username"]?.ToString(),
                    FullName = instagramPost["node"]?["user"]?["full_name"]?.ToString(),
                    ProfilePictureUrl = instagramPost["node"]?["user"]?["hd_profile_pic_url_info"]?["url"]?.ToString(),
                    IsVerified = bool.Parse(instagramPost["node"]?["user"]?["is_verified"]?.ToString())
                }
            };

            if (!string.IsNullOrEmpty(instagramPost["node"]?["carousel_media_count"]?.ToString()))
            {
                instagramPostDto.CarouselMediaCount = byte.Parse(instagramPost["node"]?["carousel_media_count"]?.ToString());
            }

            var captions = instagramPost["node"]?["caption"];
            if (!string.IsNullOrEmpty(captions?.ToString()))
            {
                instagramPostDto.Caption = new Caption
                {
                    Id = captions["pk"]?.ToString(),
                    Text = captions["text"]?.ToString(),
                    CreatedDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(captions["created_at"]?.ToString())).DateTime
                };
            }

            var location = instagramPost["node"]?["location"];
            if (!string.IsNullOrEmpty(location?.ToString()))
            {
                instagramPostDto.Location = new Location
                {
                    Lat = double.Parse(location["lat"]?.ToString()),
                    Lng = double.Parse(location["lng"]?.ToString()),
                    Name = location["name"]?.ToString()
                };
            }

            var carouselMedia = instagramPost["node"]?["carousel_media"]?.ToObject<List<object>>();

            if (carouselMedia is null)
            {
                continue;
            }

            instagramPostDto.CarouselMedia = new List<Media>();
            foreach (var media in carouselMedia)
            {
                var mediaDto = new Media
                {
                    Id = media["pk"]?.ToString(),
                    DisplayUrl = media["display_uri"]?.ToString(),
                    Dimensions = new Dimensions
                    {
                        Hieght = Convert.ToUInt16(media["original_height"]?.ToString()),
                        Width = Convert.ToUInt16(media["original_width"]?.ToString())
                    },
                    IsVideo = false
                };

                var imageVersions = media["image_versions2"]?["candidates"]?.ToObject<List<object>>();
                mediaDto.ImageUrl = imageVersions[0]?["url"]?.ToString();

                if (!string.IsNullOrEmpty(media["video_versions"]?.ToString()))
                {
                    mediaDto.IsVideo = true;
                    var videoVersions = media["video_versions"].ToObject<List<object>>();
                    mediaDto.VideoUrl = videoVersions[0]?["url"]?.ToString();
                }

                instagramPostDto.CarouselMedia.Add(mediaDto);
            }

            instagramPostsDto.Add(instagramPostDto);
        }

        return instagramPostsDto;
    }
}
