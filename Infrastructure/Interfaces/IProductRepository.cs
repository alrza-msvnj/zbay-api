using Domain.Entities;
using static Infrastructure.Dtos.ProductDto;

namespace Infrastructure.Interfaces;

public interface IProductRepository
{
    Task<ulong> CreateProduct(ProductCreateDto productCreateDto);
    Task<Product> GetProductById(ulong productId);
    Task<List<Product>> GetAllProductsByShopId(uint shopId);
    Task<List<Product>> GetAllProductsByShopId(uint shopId, ushort pageNumber, ushort pageSize);
    Task<List<Product>> GetAllProductsByCategories(List<ushort> categoryIds);
    Task<List<Product>> GetAllAvailableProducts();
    Task<ulong> UpdateProduct(ProductUpdateDto productUpdateDto);
    Task<ulong> DeleteProduct(ulong productId);
}
