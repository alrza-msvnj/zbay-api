using Domain.Entities;
using static Infrastructure.Dtos.ProductDto;

namespace Api.Factories;

public static class ProductFactory
{
    public static ProductResponseDto MapToProductResponseDto(Product product)
    {
        var productDto = new ProductResponseDto
        {
            Id = product.Id,
            Uuid = product.Uuid,
            IgId = product.IgId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            OriginalPrice = product.OriginalPrice,
            DiscountPercentage = product.DiscountPercentage,
            Stock = product.Stock,
            IgCode = product.IgCode,
            IgThumbnailSrc = product.IgThumbnailSrc,
            IgDisplayUrl = product.IgDisplayUrl,
            IgLikeCount = product.IgLikeCount,
            IgCommentCount = product.IgCommentCount,
            IgCarouselMediaCount = product.IgCarouselMediaCount,
            IgVideoUrl = product.IgVideoUrl,
            ShopId = product.ShopId,
            HasDiscount = product.HasDiscount,
            IsAvailable = product.IsAvailable,
            IsNew = product.IsNew,
            IsDeleted = product.IsDeleted,
            IgIsVideo = product.IgIsVideo,
            CreateDate = product.CreateDate,
            UpdateDate = product.UpdateDate,
            Images = product.Images,
            IgDimensions = product.IgDimensions,
            IgCaption = product.IgCaption,
            IgLocation = product.IgLocation,
            Categories = product.ProductCategories.ToList().Select(pc => pc.Category).ToList(),
            Shop = product.Shop
        };

        return productDto;
    }
}
