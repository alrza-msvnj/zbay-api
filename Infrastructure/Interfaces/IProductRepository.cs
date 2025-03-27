using Domain.Entities;
using static Infrastructure.Dtos.ProductDto;

namespace Infrastructure.Interfaces;

public interface IProductRepository
{
    Task<ulong> CreateProduct(ProductCreateDto productCreateDto);
    Task<Product> GetProductById(ulong productId);
    Task<List<Product>> GetAllProductsByShopId(uint shopId, ushort pageNumber = 1, ushort pageSize = 0);
    Task<List<Product>> GetAllProductsByCategoryIds(List<ushort> categoryIds);
    Task<List<Product>> GetAllAvailableProducts();
    Task<ulong> UpdateProduct(ProductUpdateDto productUpdateDto);
    Task<ulong> DeleteProduct(ulong productId);
}
