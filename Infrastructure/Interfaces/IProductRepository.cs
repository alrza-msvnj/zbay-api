using Domain.Entities;
using static Infrastructure.Dtos.InstagramDto;
using static Infrastructure.Dtos.ProductDto;
using static Infrastructure.Dtos.SharedDto;

namespace Infrastructure.Interfaces;

public interface IProductRepository
{
    Task<long> CreateProduct(ProductCreateDto productCreateDto);
    Task<long> CreateIgProduct(InstagramPostDto instagramPostDto, long shopId, List<int> categoryIds);
    Task<List<long>> CreateIgProducts(List<InstagramPostDto> instagramPostsDto, long shopId, List<int> categoryIds);
    Task<Product> GetProductById(long productId);
    Task<Product> GetProductByIgId(string productIgId);
    Task<List<Product>> GetAllProducts(GetAllDto getAllDto);
    Task<List<Product>> GetAllProductsByShopId(long shopId, int pageNumber, int pageSize);
    Task<List<Product>> GetAllProductsByCategoryIds(List<int> categoryIds);
    Task<List<Product>> GetAllAvailableProducts();
    Task<long> UpdateProduct(ProductUpdateDto productUpdateDto);
    Task<long> DeleteProduct(long productId);
}
