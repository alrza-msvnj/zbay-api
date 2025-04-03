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

        var productDto = new ProductResponseDto
        {
            Id = product.Id,
            Uuid = product.Uuid,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            OriginalPrice = product.OriginalPrice,
            DiscountPercentage = product.DiscountPercentage,
            Stock = product.Stock,
            Rating = product.Rating,
            Reviews = product.Reviews,
            ShopId = product.ShopId,
            HasDiscount = product.HasDiscount,
            IsAvailable = product.IsAvailable,
            IsNew = product.IsNew,
            IsDeleted = product.IsDeleted,
            CreateDate = product.CreateDate,
            UpdateDate = product.UpdateDate,
            Images = product.Images,
            Categories = product.ProductCategories.ToList().Select(pc => pc.Category).ToList()
        };

        return Ok(product);
    }

    [HttpGet(nameof(GetAllProducts))]
    public async Task<IActionResult> GetAllProducts(ushort pageNumber, ushort pageSize)
    {
        var products = await _productRepository.GetAllProducts(pageNumber, pageSize);

        var productsDto = new List<ProductResponseDto>();
        products.ForEach(p =>
        {
            var productDto = new ProductResponseDto
            {
                Id = p.Id,
                Uuid = p.Uuid,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                OriginalPrice = p.OriginalPrice,
                DiscountPercentage = p.DiscountPercentage,
                Stock = p.Stock,
                Rating = p.Rating,
                Reviews = p.Reviews,
                ShopId = p.ShopId,
                HasDiscount = p.HasDiscount,
                IsAvailable = p.IsAvailable,
                IsNew = p.IsNew,
                IsDeleted = p.IsDeleted,
                CreateDate = p.CreateDate,
                UpdateDate = p.UpdateDate,
                Images = p.Images,
                Categories = p.ProductCategories.ToList().Select(pc => pc.Category).ToList()
            };

            productsDto.Add(productDto);
        });

        return Ok(products);
    }

    [HttpGet($"{nameof(GetAllProductsByShopId)}/{{shopId}}")]
    public async Task<IActionResult> GetAllProductsByShopId(uint shopId, ushort pageNumber, ushort pageSize)
    {
        var products = await _productRepository.GetAllProductsByShopId(shopId, pageNumber, pageSize);

        var productsDto = new List<ProductResponseDto>();
        products.ForEach(p =>
        {
            var productDto = new ProductResponseDto
            {
                Id = p.Id,
                Uuid = p.Uuid,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                OriginalPrice = p.OriginalPrice,
                DiscountPercentage = p.DiscountPercentage,
                Stock = p.Stock,
                Rating = p.Rating,
                Reviews = p.Reviews,
                ShopId = p.ShopId,
                HasDiscount = p.HasDiscount,
                IsAvailable = p.IsAvailable,
                IsNew = p.IsNew,
                IsDeleted = p.IsDeleted,
                CreateDate = p.CreateDate,
                UpdateDate = p.UpdateDate,
                Images = p.Images,
                Categories = p.ProductCategories.ToList().Select(pc => pc.Category).ToList()
            };

            productsDto.Add(productDto);
        });

        return Ok(products);
    }

    [HttpPost(nameof(GetAllProductsByCategoryIds))]
    public async Task<IActionResult> GetAllProductsByCategoryIds(List<ushort> categoryIds)
    {
        var products = await _productRepository.GetAllProductsByCategoryIds(categoryIds);

        var productsDto = new List<ProductResponseDto>();
        products.ForEach(p =>
        {
            var productDto = new ProductResponseDto
            {
                Id = p.Id,
                Uuid = p.Uuid,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                OriginalPrice = p.OriginalPrice,
                DiscountPercentage = p.DiscountPercentage,
                Stock = p.Stock,
                Rating = p.Rating,
                Reviews = p.Reviews,
                ShopId = p.ShopId,
                HasDiscount = p.HasDiscount,
                IsAvailable = p.IsAvailable,
                IsNew = p.IsNew,
                IsDeleted = p.IsDeleted,
                CreateDate = p.CreateDate,
                UpdateDate = p.UpdateDate,
                Images = p.Images,
                Categories = p.ProductCategories.ToList().Select(pc => pc.Category).ToList()
            };

            productsDto.Add(productDto);
        });

        return Ok(products);
    }

    [HttpGet(nameof(GetAllAvailableProducts))]
    public async Task<IActionResult> GetAllAvailableProducts()
    {
        var products = await _productRepository.GetAllAvailableProducts();

        var productsDto = new List<ProductResponseDto>();
        products.ForEach(p =>
        {
            var productDto = new ProductResponseDto
            {
                Id = p.Id,
                Uuid = p.Uuid,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                OriginalPrice = p.OriginalPrice,
                DiscountPercentage = p.DiscountPercentage,
                Stock = p.Stock,
                Rating = p.Rating,
                Reviews = p.Reviews,
                ShopId = p.ShopId,
                HasDiscount = p.HasDiscount,
                IsAvailable = p.IsAvailable,
                IsNew = p.IsNew,
                IsDeleted = p.IsDeleted,
                CreateDate = p.CreateDate,
                UpdateDate = p.UpdateDate,
                Images = p.Images,
                Categories = p.ProductCategories.ToList().Select(pc => pc.Category).ToList()
            };

            productsDto.Add(productDto);
        });

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
