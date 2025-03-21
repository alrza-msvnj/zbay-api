using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.Dtos.ShopDto;

namespace Infrastructure.Repositories;

public class ShopRepository : IShopRepository
{
    #region Initialization

    private readonly Context _context;

    public ShopRepository(Context context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<uint> CreateShop(ShopCreateDto shopCreateDto)
    {
        var shop = new Shop
        {
            Uuid = Guid.NewGuid(),
            InstagramId = shopCreateDto.InstagramId,
            InstagramUrl = shopCreateDto.InstagramUrl,
            Name = shopCreateDto.Name,
            Followers = shopCreateDto.Followers,
            Logo = shopCreateDto.Logo,
            IsVerified = shopCreateDto.IsVerified,
            IsValidated = false,
            IsDeleted = false,
            JoinDate = DateTime.UtcNow
        };

        await _context.AddAsync(shop);
        await _context.SaveChangesAsync();

        return shop.Id;
    }

    public async Task<Shop> GetShopById(uint shopId)
    {
        return await _context.Shop.FindAsync(shopId);
    }

    public async Task<Shop> GetShopByUserId(uint userId)
    {
        return await _context.Shop.FirstOrDefaultAsync(s => s.OwnerId == userId);
    }

    public async Task<List<Shop>> GetAllShopsByPaging(ushort pageNumber, ushort pageSize)
    {
        return await _context.Shop.Skip((pageNumber - 1) * pageSize).ToListAsync();
    }

    public async Task<List<Shop>> GetAllUnvalidatedShops()
    {
        return await _context.Shop.Where(s => s.IsValidated == false).ToListAsync();
    }

    public async Task<uint> DeleteShop(uint shopId)
    {
        var shop = await GetShopById(shopId);

        shop.IsDeleted = true;

        await _context.SaveChangesAsync();

        return shop.Id;
    }

    #endregion
}
