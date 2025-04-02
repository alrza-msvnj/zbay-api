using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Infrastructure.Dtos.ShopDto;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ShopController : ControllerBase
{
    #region Initialization

    private readonly IShopRepository _shopRepository;

    public ShopController(IShopRepository shopRepository)
    {
        _shopRepository = shopRepository;
    }

    #endregion

    #region Apis

    [HttpPost(nameof(CreateShop))]
    public async Task<IActionResult> CreateShop(ShopCreateDto shopCreateDto)
    {
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
            Rating = shop.Rating,
            Reviews = shop.Reviews,
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
            Rating = shop.Rating,
            Reviews = shop.Reviews,
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

    [HttpGet(nameof(GetAllShopsByPaging))]
    public async Task<IActionResult> GetAllShopsByPaging(ushort pageNumber, ushort pageSize)
    {
        var shops = await _shopRepository.GetAllShopsByPaging(pageNumber, pageSize);

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
                Rating = s.Rating,
                Reviews = s.Reviews,
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
                Rating = s.Rating,
                Reviews = s.Reviews,
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

    [HttpDelete($"{nameof(DeleteShop)}/{{shopId}}")]
    public async Task<IActionResult> DeleteShop(uint shopId)
    {
        await _shopRepository.DeleteShop(shopId);

        return Ok(shopId);
    }

    #endregion
}
