using Api.Factories;
using Api.Services;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Infrastructure.Dtos.ProductDto;
using static Infrastructure.Dtos.SharedDto;
using static Infrastructure.Dtos.ShopDto;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    #region Initialization

    private readonly IProductRepository _productRepository;
    private readonly IShopRepository _shopRepository;
    private readonly IInstagramScraperService _instagramScraperService;

    public ProductController(IProductRepository productRepository, IShopRepository shopRepository, IInstagramScraperService instagramScraperService)
    {
        _productRepository = productRepository;
        _shopRepository = shopRepository;
        _instagramScraperService = instagramScraperService;
    }

    #endregion

    #region Apis

    [HttpPost(nameof(CreateProduct))]
    public async Task<IActionResult> CreateProduct(ProductCreateDto productCreateDto)
    {
        var productId = await _productRepository.CreateProduct(productCreateDto);

        return Ok(productId);
    }

    [HttpPost(nameof(CreateIgProducts))]
    public async Task<IActionResult> CreateIgProducts(ProductCreateIgDto productCreateIgDto)
    {
        var productIds = new List<ulong>();
        foreach (var username in productCreateIgDto.Usernames)
        {
            var instagramPostsDto = await _instagramScraperService.ScrapePosts(username, 1);

            var igId = instagramPostsDto[0].Owner.Id;
            var existedShopByIgId = await _shopRepository.GetShopByIgId(igId);

            var shopId = existedShopByIgId?.Id;
            if (existedShopByIgId is null)
            {
                var shopCreateDto = new ShopCreateDto
                {
                    IgId = instagramPostsDto[0]?.Owner?.Id,
                    Name = instagramPostsDto[0]?.Owner?.FullName,
                    Logo = instagramPostsDto[0]?.Owner?.ProfilePictureUrl,
                    IgUsername = instagramPostsDto[0]?.Owner?.Username,
                    IgFullName = instagramPostsDto[0]?.Owner?.FullName,
                    IgFollowers = instagramPostsDto[0]?.Owner?.Followers,
                    OwnerId = 0,
                    IsVerified = (bool)(instagramPostsDto[0]?.Owner?.IsVerified)
                };

                shopId = await _shopRepository.CreateShop(shopCreateDto);
            }

            productIds.AddRange(await _productRepository.CreateIgProducts(instagramPostsDto, shopId.Value, productCreateIgDto.CategoryIds));
        }

        return Ok(productIds);
    }

    [HttpGet($"{nameof(GetProductById)}/{{productId}}")]
    public async Task<IActionResult> GetProductById(ulong productId)
    {
        var product = await _productRepository.GetProductById(productId);

        var productDto = ProductFactory.MapToProductResponseDto(product);

        return Ok(productDto);
    }

    [HttpPost(nameof(GetAllProducts))]
    public async Task<IActionResult> GetAllProducts(GetAllDto getAllDto)
    {
        var products = await _productRepository.GetAllProducts(getAllDto);

        var productsDto = new List<ProductResponseDto>();
        products.ForEach(p =>
        {
            var productDto = ProductFactory.MapToProductResponseDto(p);

            productsDto.Add(productDto);
        });

        return Ok(productsDto);
    }

    [HttpGet($"{nameof(GetAllProductsByShopId)}/{{shopId}}")]
    public async Task<IActionResult> GetAllProductsByShopId(uint shopId, ushort pageNumber, ushort pageSize)
    {
        var products = await _productRepository.GetAllProductsByShopId(shopId, pageNumber, pageSize);

        var productsDto = new List<ProductResponseDto>();
        products.ForEach(p =>
        {
            var productDto = ProductFactory.MapToProductResponseDto(p);

            productsDto.Add(productDto);
        });

        return Ok(productsDto);
    }

    [HttpPost(nameof(GetAllProductsByCategoryIds))]
    public async Task<IActionResult> GetAllProductsByCategoryIds(List<ushort> categoryIds)
    {
        var products = await _productRepository.GetAllProductsByCategoryIds(categoryIds);

        var productsDto = new List<ProductResponseDto>();
        products.ForEach(p =>
        {
            var productDto = ProductFactory.MapToProductResponseDto(p);

            productsDto.Add(productDto);
        });

        return Ok(productsDto);
    }

    [HttpGet(nameof(GetAllAvailableProducts))]
    public async Task<IActionResult> GetAllAvailableProducts()
    {
        var products = await _productRepository.GetAllAvailableProducts();

        var productsDto = new List<ProductResponseDto>();
        products.ForEach(p =>
        {
            var productDto = ProductFactory.MapToProductResponseDto(p);

            productsDto.Add(productDto);
        });

        return Ok(productsDto);
    }

    [HttpPut(nameof(UpdateProduct))]
    public async Task<IActionResult> UpdateProduct(ProductUpdateDto productUpdateDto)
    {
        await _productRepository.UpdateProduct(productUpdateDto);

        return Ok(productUpdateDto);
    }

    [HttpDelete($"{nameof(DeleteProduct)}/{{productId}}")]
    public async Task<IActionResult> DeleteProduct(ulong productId)
    {
        await _productRepository.DeleteProduct(productId);

        return Ok(productId);
    }

    #endregion
}
