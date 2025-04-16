using Domain.Entities;
using static Infrastructure.Dtos.ShopDto;

namespace Api.Factories;

public static class ShopFactory
{
    public static ShopResponseDto MapToShopResponseDto(Shop shop)
    {
        var shopDto = new ShopResponseDto
        {
            Id = shop.Id,
            Uuid = shop.Uuid,
            IgId = shop.IgId,
            Name = shop.Name,
            Description = shop.Description,
            Logo = shop.Logo,
            TotalProducts = shop.TotalProducts,
            IgUsername = shop.IgUsername,
            IgFullName = shop.IgFullName,
            IgFollowers = shop.IgFollowers,
            OwnerId = shop.OwnerId,
            IsVerified = shop.IsVerified,
            IsValidated = shop.IsValidated,
            IsDeleted = shop.IsDeleted,
            JoinDate = shop.JoinDate,
            Categories = shop.ShopCategories.ToList().Select(sc => sc.Category).ToList(),
        };

        return shopDto;
    }
}
