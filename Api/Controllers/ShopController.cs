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

        return Ok(shop);
    }

    [HttpGet($"{nameof(GetShopByUserId)}/{{userId}}")]
    public async Task<IActionResult> GetShopByUserId(uint userId)
    {
        var shop = await _shopRepository.GetShopByUserId(userId);

        return Ok(shop);
    }

    [HttpGet($"{nameof(GetAllShopsByPaging)}/{{pageNumber}}/{{pageSize}}")]
    public async Task<IActionResult> GetAllShopsByPaging(ushort pageNumber, ushort pageSize)
    {
        var shops = await _shopRepository.GetAllShopsByPaging(pageNumber, pageSize);

        return Ok(shops);
    }

    [HttpGet(nameof(GetAllUnvalidatedShops))]
    public async Task<IActionResult> GetAllUnvalidatedShops()
    {
        var shops = await _shopRepository.GetAllUnvalidatedShops();

        return Ok(shops);
    }

    [HttpDelete(nameof(DeleteShop))]
    public async Task<IActionResult> DeleteShop(uint shopId)
    {
        await _shopRepository.DeleteShop(shopId);

        return Ok(shopId);
    }

    #endregion
}
