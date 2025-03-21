using Domain.Entities;

namespace Application.Interfaces;

public interface IShopRepository
{
    Task<uint> CreateShop(ShopCreateDto shopCreateDto);
    Task<Shop> GetShopById(uint shopId);
    Task<Shop> GetShopByUserId(uint userId);
    Task<List<Shop>> GetAllShopsByPaging(ushort pageNumber, ushort pageSize);
    Task<List<Shop>> GetAllUnvalidatedShops();
    Task<uint> DeleteShop(uint shopId);
}
