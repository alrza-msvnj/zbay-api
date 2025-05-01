using Domain.Entities;
using static Infrastructure.Dtos.SharedDto;
using static Infrastructure.Dtos.ShopDto;

namespace Application.Interfaces;

public interface IShopRepository
{
    Task<long> CreateShop(ShopCreateDto shopCreateDto);
    Task<Shop> GetShopById(long shopId);
    Task<Shop> GetShopByOwnerId(long ownerId);
    Task<Shop> GetShopByIgId(string igId);
    Task<Shop> GetShopByIgUsername(string igUsername);
    Task<List<Shop>> GetAllShops(GetAllDto shopGetAllDto);
    Task<List<Shop>> GetAllUnvalidatedShops();
    Task<long> DeleteShop(long shopId);
    Task<long> ApproveOrRejectShop(long shopId, bool isApproved);
}
