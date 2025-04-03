using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.Dtos.ShopDto;

namespace Infrastructure.Repositories;

public class ShopRepository : IShopRepository
{
    #region Initialization

    private readonly Context _context;
    private readonly IUserRepository _userRepository;

    public ShopRepository(Context context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    #endregion

    #region Methods

    public async Task<uint> CreateShop(ShopCreateDto shopCreateDto)
    {
        var owner = await _userRepository.GetUserById(shopCreateDto.OwnerId);

        if (owner is null)
        {
            throw new InvalidOperationException("Owner does not exist.");
        }

        var shop = new Shop
        {
            Uuid = Guid.NewGuid(),
            InstagramId = shopCreateDto.InstagramId,
            InstagramUrl = shopCreateDto.InstagramUrl,
            Name = shopCreateDto.Name,
            Followers = shopCreateDto.Followers,
            Logo = shopCreateDto.Logo,
            OwnerId = shopCreateDto.OwnerId,
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
        return await _context.Shop.Where(s => s.Id == shopId).Include(s => s.ShopCategories).ThenInclude(sc => sc.Category).FirstOrDefaultAsync();
    }

    public async Task<Shop> GetShopByOwnerId(uint ownerId)
    {
        return await _context.Shop.Where(s => s.IsDeleted == false && s.OwnerId == ownerId).Include(s => s.ShopCategories).ThenInclude(sc => sc.Category).FirstOrDefaultAsync();
    }

    public async Task<List<Shop>> GetAllShopsByPaging(ushort pageNumber, ushort pageSize)
    {
        return await _context.Shop.Where(s => s.IsDeleted == false).Skip((pageNumber - 1) * pageSize).Include(s => s.ShopCategories).ThenInclude(sc => sc.Category).ToListAsync();
    }

    public async Task<List<Shop>> GetAllShopsByCategoryIds(List<ushort> categoryIds)
    {
        return await _context.Shop.Join(_context.ShopCategory, s => s.Id, sc => sc.ShopId, (s, sc) => new { s, sc }).Where(ssc => categoryIds.Contains(ssc.sc.CategoryId)).Select(ssc => ssc.s).Include(s => s.ShopCategories).ThenInclude(sc => sc.Category).ToListAsync();
    }

    public async Task<List<Shop>> GetAllUnvalidatedShops()
    {
        return await _context.Shop.Where(s => s.IsDeleted == false && s.IsValidated == false).Include(s => s.ShopCategories).ThenInclude(sc => sc.Category).ToListAsync();
    }

    public async Task<uint> DeleteShop(uint shopId)
    {
        var shop = await GetShopById(shopId);

        if (shop is null)
        {
            throw new InvalidOperationException("Shop does not exist.");
        }

        shop.IsDeleted = true;

        await _context.SaveChangesAsync();

        return shop.Id;
    }

    #endregion
}
