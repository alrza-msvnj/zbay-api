using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using static Infrastructure.Dtos.SharedDto;
using static Infrastructure.Dtos.ShopDto;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ShopController : ControllerBase
{
    #region Initialization

    private readonly IShopRepository _shopRepository;
    private readonly HttpClient _httpClient;
    private const string AccessToken = "IGAAOtkoELzZCpBZAE9WUm5NbGdKT2ZAzMzJiemhMVzlYSktZATXM1V1kyOG1hV2phcGNfUnZAWMDhZAWVhYWVJ2djJVdkVMWkNuY282STYwbWF6ZAm9PTUJGdWRKVWdqeHRwVmtJUGRqbGJzM3lIN2xpVTFsaHNrTkpaUnEtWnlPQmdnOAZDZD";

    public ShopController(IShopRepository shopRepository, HttpClient httpClient)
    {
        _shopRepository = shopRepository;
        _httpClient = httpClient;
    }

    #endregion

    #region Apis

    [Authorize]
    [HttpPost(nameof(CreateShop))]
    public async Task<IActionResult> CreateShop(ShopCreateDto shopCreateDto)
    {
        var shopId = await _shopRepository.CreateShop(shopCreateDto);

        return Ok(shopId);
    }

    [Authorize]
    [HttpPost(nameof(CreateShopByInstagram))]
    public async Task<IActionResult> CreateShopByInstagram()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.instagram.com/v22.0/me?fields=user_id,biography,username,name,profile_picture_url,followers_count&access_token={AccessToken}");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(500, "Server error.");
        }

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        var shopCreateDto = new ShopCreateDto
        {
            IgId = data["user_id"],
            InstagramId = data["username"],
            InstagramUrl = $"https://www.instagram.com/{data["username"]}",
            Name = data["name"],
            Description = data["biography"],
            Followers = Convert.ToUInt32(data["followers_count"]),
            Logo = data["profile_picture_url"],
            OwnerId = Convert.ToUInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            //IsVerified = false
        };

        var shopId = await _shopRepository.CreateShop(shopCreateDto);

        return Ok(shopId);
    }

    [HttpGet($"{nameof(GetShopById)}/{{shopId}}")]
    public async Task<IActionResult> GetShopById(uint shopId)
    {
        var shop = await _shopRepository.GetShopById(shopId);

        var shopDto = new ShopResponseDto
        {
            Id = shop.Id,
            Uuid = shop.Uuid,
            InstagramId = shop.InstagramId,
            InstagramUrl = shop.InstagramUrl,
            Name = shop.Name,
            Description = shop.Description,
            Followers = shop.Followers,
            Logo = shop.Logo,
            TotalProducts = shop.TotalProducts,
            OwnerId = shop.OwnerId,
            IsVerified = shop.IsVerified,
            IsValidated = shop.IsValidated,
            IsDeleted = shop.IsDeleted,
            JoinDate = shop.JoinDate,
            Categories = shop.ShopCategories.ToList().Select(sc => sc.Category).ToList(),
        };

        return Ok(shopDto);
    }

    [HttpGet($"{nameof(GetShopByUserId)}/{{userId}}")]
    public async Task<IActionResult> GetShopByUserId(uint userId)
    {
        var shop = await _shopRepository.GetShopByOwnerId(userId);

        var shopDto = new ShopResponseDto
        {
            Id = shop.Id,
            Uuid = shop.Uuid,
            InstagramId = shop.InstagramId,
            InstagramUrl = shop.InstagramUrl,
            Name = shop.Name,
            Description = shop.Description,
            Followers = shop.Followers,
            Logo = shop.Logo,
            TotalProducts = shop.TotalProducts,
            OwnerId = shop.OwnerId,
            IsVerified = shop.IsVerified,
            IsValidated = shop.IsValidated,
            IsDeleted = shop.IsDeleted,
            JoinDate = shop.JoinDate,
            Categories = shop.ShopCategories.ToList().Select(sc => sc.Category).ToList(),
        };

        return Ok(shopDto);
    }

    [HttpPost(nameof(GetAllShops))]
    public async Task<IActionResult> GetAllShops(GetAllDto shopGetAllDto)
    {
        var shops = await _shopRepository.GetAllShops(shopGetAllDto);

        var shopsDto = new List<ShopResponseDto>();
        shops.ForEach(s =>
        {
            var shopDto = new ShopResponseDto
            {
                Id = s.Id,
                Uuid = s.Uuid,
                InstagramId = s.InstagramId,
                InstagramUrl = s.InstagramUrl,
                Name = s.Name,
                Description = s.Description,
                Followers = s.Followers,
                Logo = s.Logo,
                TotalProducts = s.TotalProducts,
                OwnerId = s.OwnerId,
                IsVerified = s.IsVerified,
                IsValidated = s.IsValidated,
                IsDeleted = s.IsDeleted,
                JoinDate = s.JoinDate,
                Categories = s.ShopCategories.ToList().Select(sc => sc.Category).ToList(),
            };

            shopsDto.Add(shopDto);
        });

        return Ok(shopsDto);
    }

    [HttpGet(nameof(GetAllUnvalidatedShops))]
    public async Task<IActionResult> GetAllUnvalidatedShops()
    {
        var shops = await _shopRepository.GetAllUnvalidatedShops();

        var shopsDto = new List<ShopResponseDto>();
        shops.ForEach(s =>
        {
            var shopDto = new ShopResponseDto
            {
                Id = s.Id,
                Uuid = s.Uuid,
                InstagramId = s.InstagramId,
                InstagramUrl = s.InstagramUrl,
                Name = s.Name,
                Description = s.Description,
                Followers = s.Followers,
                Logo = s.Logo,
                TotalProducts = s.TotalProducts,
                OwnerId = s.OwnerId,
                IsVerified = s.IsVerified,
                IsValidated = s.IsValidated,
                IsDeleted = s.IsDeleted,
                JoinDate = s.JoinDate,
                Categories = s.ShopCategories.ToList().Select(sc => sc.Category).ToList(),
            };

            shopsDto.Add(shopDto);
        });

        return Ok(shopsDto);
    }

    [HttpGet(nameof(GetShopInstagramMediaObjects))]
    public async Task<IActionResult> GetShopInstagramMediaObjects(uint shopId)
    {
        var shop = await _shopRepository.GetShopById(shopId);

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.instagram.com/v22.0/{shop.IgId}/media?access_token={AccessToken}");

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        return Ok();
    }

    [HttpDelete($"{nameof(DeleteShop)}/{{shopId}}")]
    public async Task<IActionResult> DeleteShop(uint shopId)
    {
        await _shopRepository.DeleteShop(shopId);

        return Ok(shopId);
    }

    [HttpGet($"{nameof(ApproveOrRejectShop)}/{{shopId}}/{{isApproved}}")]
    public async Task<IActionResult> ApproveOrRejectShop(uint shopId, bool isApproved)
    {
        await _shopRepository.ApproveOrRejectShop(shopId, isApproved);

        return Ok(shopId);
    }

    #endregion
}
