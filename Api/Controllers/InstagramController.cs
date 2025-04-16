using Api.Factories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InstagramController : ControllerBase
{
    #region Initialization

    private readonly HttpClient _httpClient = new();
    private const string InstagramApiBaseUrl = "https://www.instagram.com/graphql/query";
    private readonly string TestAccessToken;
    private readonly string InstagramDocumentId;

    public InstagramController(IConfiguration configuration)
    {
        TestAccessToken = configuration.GetValue<string>("InstagramApp:TestAccessToken");
        InstagramDocumentId = configuration.GetValue<string>("InstagramApi:InstagramDocumentId");
    }

    #endregion

    #region Apis

    [HttpPost(nameof(ScrapePost))]
    public async Task<IActionResult> ScrapePost([FromBody] string postUrlOrPostShortCode)
    {
        string postShortCode;
        if (postUrlOrPostShortCode.Contains("http"))
        {
            postShortCode = postUrlOrPostShortCode.Split("/p/").Last().Split('/').First();
        }
        else
        {
            postShortCode = postUrlOrPostShortCode;
        }

        Console.WriteLine($"Scraping instagram post: {postShortCode}");

        var variables = new Dictionary<string, object?>
        {
            { "shortcode", postShortCode },
            { "fetch_tagged_user_count", null },
            { "hoisted_comment_id", null },
            { "hoisted_reply_id", null }
        };
        var jsonVariables = JsonConvert.SerializeObject(variables, Formatting.None, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include
        });
        var encodedVariables = WebUtility.UrlEncode(jsonVariables);
        var body = $"variables={encodedVariables}&doc_id={InstagramDocumentId}";

        var request = new HttpRequestMessage(HttpMethod.Post, InstagramApiBaseUrl) 
        {
            Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded")
        };

        var response = await _httpClient.SendAsync(request);
        var result = await response.Content.ReadAsStringAsync();

        var instagramPost = InstagramFactory.MapToInstagramPostDto(result);

        return Ok(instagramPost);
    }

    //[Authorize]
    //[HttpPost(nameof(CreateShopByInstagram))]
    //public async Task<IActionResult> CreateShopByInstagram()
    //{
    //    var request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.instagram.com/v22.0/me?fields=user_id,biography,username,name,profile_picture_url,followers_count&access_token={AccessToken}");

    //    var response = await _httpClient.SendAsync(request);

    //    if (!response.IsSuccessStatusCode)
    //    {
    //        return StatusCode(500, "Server error.");
    //    }

    //    var json = await response.Content.ReadAsStringAsync();
    //    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

    //    var shopCreateDto = new ShopCreateDto
    //    {
    //        IgId = data["user_id"],
    //        InstagramId = data["username"],
    //        InstagramUrl = $"https://www.instagram.com/{data["username"]}",
    //        Name = data["name"],
    //        Description = data["biography"],
    //        Followers = Convert.ToUInt32(data["followers_count"]),
    //        Logo = data["profile_picture_url"],
    //        OwnerId = Convert.ToUInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
    //        //IsVerified = false
    //    };

    //    var shopId = await _shopRepository.CreateShop(shopCreateDto);

    //    return Ok(shopId);
    //}

    //[HttpGet(nameof(GetShopInstagramMediaObjects))]
    //public async Task<IActionResult> GetShopInstagramMediaObjects(uint shopId)
    //{
    //    var shop = await _shopRepository.GetShopById(shopId);

    //    var request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.instagram.com/v22.0/{shop.IgId}/media?access_token={AccessToken}");

    //    var response = await _httpClient.SendAsync(request);
    //    var json = await response.Content.ReadAsStringAsync();
    //    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

    //    return Ok();
    //}

    #endregion
}
