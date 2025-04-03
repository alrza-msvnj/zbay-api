using Domain.Entities;
using static Infrastructure.Dtos.ProductDto;
using static Infrastructure.Dtos.SharedDto;

namespace Infrastructure.Interfaces;

public interface IProductRepository
{
    Task<ulong> CreateProduct(ProductCreateDto productCreateDto);
    Task<Product> GetProductById(ulong productId);
    Task<List<Product>> GetAllProducts(GetAllDto getAllDto);
    Task<List<Product>> GetAllProductsByShopId(uint shopId, ushort pageNumber, ushort pageSize);
    Task<List<Product>> GetAllProductsByCategoryIds(List<ushort> categoryIds);
    Task<List<Product>> GetAllAvailableProducts();
    Task<ulong> UpdateProduct(ProductUpdateDto productUpdateDto);
    Task<ulong> DeleteProduct(ulong productId);
}
