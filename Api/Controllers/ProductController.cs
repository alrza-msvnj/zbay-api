using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Infrastructure.Dtos.ProductDto;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    #region Initialization

    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    #endregion

    #region Apis

    [HttpPost(nameof(CreateProduct))]
    public async Task<IActionResult> CreateProduct(ProductCreateDto productCreateDto)
    {
        var productId = await _productRepository.CreateProduct(productCreateDto);

        return Ok(productId);
    }

    [HttpGet($"{nameof(GetProductById)}/{{productId}}")]
    public async Task<IActionResult> GetProductById(ulong productId)
    {
        var product = await _productRepository.GetProductById(productId);

        return Ok(product);
    }

    [HttpGet($"{nameof(GetAllProductsByShopId)}/{{shopId}}")]
    public async Task<IActionResult> GetAllProductsByShopId(uint shopId, ushort pageNumber = 1, ushort pageSize = 0)
    {
        var products = await _productRepository.GetAllProductsByShopId(shopId, pageNumber, pageSize);

        return Ok(products);
    }

    [HttpPost(nameof(GetAllProductsByCategoryIds))]
    public async Task<IActionResult> GetAllProductsByCategoryIds(List<ushort> categoryIds)
    {
        var products = await _productRepository.GetAllProductsByCategoryIds(categoryIds);

        return Ok(products);
    }

    [HttpGet(nameof(GetAllAvailableProducts))]
    public async Task<IActionResult> GetAllAvailableProducts()
    {
        var products = await _productRepository.GetAllAvailableProducts();

        return Ok(products);
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
