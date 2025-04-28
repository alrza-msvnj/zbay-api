using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Infrastructure.Dtos.SharedDto;
using static Infrastructure.Dtos.ShopDto;

namespace Infrastructure.Repositories;

public class ShopRepository : IShopRepository
{
    #region Initialization

    private readonly Context _context;
    private readonly IUserRepository _userRepository;
    private const string LogoPath = @"G:\Projects\Z Market\src\Front\zmarket-front\public\logos\";

    public ShopRepository(Context context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    #endregion

    #region Methods

    public async Task<uint> CreateShop(ShopCreateDto shopCreateDto)
    {
        if (shopCreateDto.OwnerId is not null)
        {
            var owner = await _userRepository.GetUserById(shopCreateDto.OwnerId.Value);

            if (owner is null)
            {
                throw new InvalidOperationException("Owner does not exist.");
            }
        }

        var shop = new Shop
        {
            Uuid = Guid.NewGuid(),
            IgId = shopCreateDto.IgId,
            Name = shopCreateDto.Name,
            Description = shopCreateDto.Description,
            Logo = shopCreateDto.Logo,
            IgUsername = shopCreateDto.IgUsername,
            IgFullName = shopCreateDto.IgFullName,
            IgFollowers = shopCreateDto.IgFollowers,
            OwnerId = shopCreateDto.OwnerId,
            IsVerified = shopCreateDto.IsVerified,
            IsValidated = false,
            IsDeleted = false,
            JoinDate = DateTime.UtcNow
        };

        var logoName = Guid.NewGuid().ToString() + ".png";
        var imagePath = $@"{LogoPath}\{logoName}";
        using (HttpClient client = new HttpClient())
        {
            try
            {
                using (HttpResponseMessage response = await client.GetAsync(shop.Logo))
                {
                    response.EnsureSuccessStatusCode();

                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                    await File.WriteAllBytesAsync(imagePath, imageBytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error downloading image: {ex.Message}");
            }
        }

        shop.Logo = logoName;

        await _context.AddAsync(shop);
        await _context.SaveChangesAsync();

        if (shopCreateDto.OwnerId is not null)
        {
            var user = await _userRepository.GetUserById(shopCreateDto.OwnerId.Value);

            user.ShopId = shop.Id;
            user.Role = UserRole.ShopOwner;

            await _context.SaveChangesAsync();
        }

        return shop.Id;
    }

    public async Task<Shop> GetShopById(uint shopId)
    {
        return await _context.Shop.Where(s => s.Id == shopId).Include(s => s.ShopCategories).ThenInclude(sc => sc.Category).FirstOrDefaultAsync();
    }

    public async Task<Shop> GetShopByOwnerId(uint ownerId)
    {
        return await _context.Shop.Where(s => !s.IsDeleted && s.OwnerId == ownerId).Include(s => s.ShopCategories).ThenInclude(sc => sc.Category).FirstOrDefaultAsync();
    }

    public async Task<Shop> GetShopByIgId(string igId)
    {
        return await _context.Shop.FirstOrDefaultAsync(s => s.IgId == igId);
    }

    public async Task<Shop> GetShopByIgUsername(string igUsername)
    {
        return await _context.Shop.FirstOrDefaultAsync(s => s.IgUsername == igUsername);
    }

    public async Task<List<Shop>> GetAllShops(GetAllDto getAllDto)
    {
        if (getAllDto.CategoryIds is null || getAllDto.CategoryIds.IsNullOrEmpty())
        {
            var query = _context.Shop.Where(s => !s.IsDeleted && s.IsValidated && s.Name.Contains(getAllDto.SearchTerm));

            if (getAllDto.PageNumber > 0 && getAllDto.PageSize > 0)
            {
                query = query
                    .Skip((getAllDto.PageNumber - 1) * getAllDto.PageSize)
                    .Take(getAllDto.PageSize);
            }

            return await query.Include(s => s.ShopCategories).ThenInclude(sc => sc.Category).ToListAsync();
        }

        var shopIdsQuery = _context.Shop
            .Join(_context.ShopCategory, s => s.Id, sc => sc.ShopId, (s, sc) => new { s, sc })
            .Where(ssc => !ssc.s.IsDeleted && ssc.s.IsValidated && getAllDto.CategoryIds.Contains(ssc.sc.CategoryId) && ssc.s.Name.Contains(getAllDto.SearchTerm))
            .GroupBy(ssc => ssc.s.Id)
            .Select(g => g.Key);

        if (getAllDto.PageNumber > 0 && getAllDto.PageSize > 0)
        {
            shopIdsQuery = shopIdsQuery
                .Skip((getAllDto.PageNumber - 1) * getAllDto.PageSize)
                .Take(getAllDto.PageSize);
        }

        var shopIds = await shopIdsQuery.ToListAsync();

        return await _context.Shop
            .Where(s => shopIds.Contains(s.Id))
            .Include(s => s.ShopCategories)
            .ThenInclude(sc => sc.Category)
            .ToListAsync();
    }

    public async Task<List<Shop>> GetAllUnvalidatedShops()
    {
        return await _context.Shop.Where(s => !s.IsDeleted && !s.IsValidated).Include(s => s.ShopCategories).ThenInclude(sc => sc.Category).ToListAsync();
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

    public async Task<uint> ApproveOrRejectShop(uint shopId, bool isApproved)
    {
        var shop = await GetShopById(shopId);

        if (shop is null)
        {
            throw new InvalidOperationException("Shop does not exist.");
        }

        shop.IsValidated = isApproved;

        await _context.SaveChangesAsync();

        return shop.Id;
    }

    #endregion
}
