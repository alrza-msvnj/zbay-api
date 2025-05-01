using Api.Factories;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Infrastructure.Dtos.SharedDto;
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

    [Authorize]
    [HttpPost(nameof(CreateShop))]
    public async Task<IActionResult> CreateShop(ShopCreateDto shopCreateDto)
    {
        var shopId = await _shopRepository.CreateShop(shopCreateDto);

        return Ok(shopId);
    }

    [HttpGet($"{nameof(GetShopById)}/{{shopId}}")]
    public async Task<IActionResult> GetShopById(long shopId)
    {
        var shop = await _shopRepository.GetShopById(shopId);

        var shopDto = ShopFactory.MapToShopResponseDto(shop);

        return Ok(shopDto);
    }

    [HttpGet($"{nameof(GetShopByUserId)}/{{userId}}")]
    public async Task<IActionResult> GetShopByUserId(long userId)
    {
        var shop = await _shopRepository.GetShopByOwnerId(userId);

        var shopDto = ShopFactory.MapToShopResponseDto(shop);

        return Ok(shopDto);
    }

    [HttpPost(nameof(GetAllShops))]
    public async Task<IActionResult> GetAllShops(GetAllDto shopGetAllDto)
    {
        var shops = await _shopRepository.GetAllShops(shopGetAllDto);

        var shopsDto = new List<ShopResponseDto>();
        shops.ForEach(s =>
        {
            var shopDto = ShopFactory.MapToShopResponseDto(s);

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
            var shopDto = ShopFactory.MapToShopResponseDto(s);

            shopsDto.Add(shopDto);
        });

        return Ok(shopsDto);
    }

    [HttpDelete($"{nameof(DeleteShop)}/{{shopId}}")]
    public async Task<IActionResult> DeleteShop(long shopId)
    {
        await _shopRepository.DeleteShop(shopId);

        return Ok(shopId);
    }

    [HttpGet($"{nameof(ApproveOrRejectShop)}/{{shopId}}/{{isApproved}}")]
    public async Task<IActionResult> ApproveOrRejectShop(long shopId, bool isApproved)
    {
        await _shopRepository.ApproveOrRejectShop(shopId, isApproved);

        return Ok(shopId);
    }

    #endregion
}
