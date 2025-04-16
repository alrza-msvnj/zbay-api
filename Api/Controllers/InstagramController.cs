using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InstagramController : ControllerBase
{
    #region Initialization

    private readonly HttpClient _httpClient = new();
    private readonly string TestAccessToken;

    public InstagramController(IConfiguration configuration)
    {
        TestAccessToken = configuration.GetValue<string>("InstagramApp:TestAccessToken");
    }

    #endregion

    #region Apis

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
