using Domain.Entities;
using static Infrastructure.Dtos.ShopDto;

namespace Application.Interfaces;

public interface IShopRepository
{
    Task<uint> CreateShop(ShopCreateDto shopCreateDto);
    Task<Shop> GetShopById(uint shopId);
    Task<Shop> GetShopByOwnerId(uint ownerId);
    Task<List<Shop>> GetAllShops(ushort pageNumber, ushort pageSize);
    Task<List<Shop>> GetAllShopsByCategoryIds(List<ushort> categoryIds);
    Task<List<Shop>> GetAllUnvalidatedShops();
    Task<uint> DeleteShop(uint shopId);
}
