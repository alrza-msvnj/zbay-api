using Domain.Entities;
using static Infrastructure.Dtos.SharedDto;
using static Infrastructure.Dtos.ShopDto;

namespace Application.Interfaces;

public interface IShopRepository
{
    Task<uint> CreateShop(ShopCreateDto shopCreateDto);
    Task<Shop> GetShopById(uint shopId);
    Task<Shop> GetShopByOwnerId(uint ownerId);
    Task<Shop> GetShopByIgId(string igId);
    Task<Shop> GetShopByIgUsername(string igUsername);
    Task<List<Shop>> GetAllShops(GetAllDto shopGetAllDto);
    Task<List<Shop>> GetAllUnvalidatedShops();
    Task<uint> DeleteShop(uint shopId);
    Task<uint> ApproveOrRejectShop(uint shopId, bool isApproved);
}
