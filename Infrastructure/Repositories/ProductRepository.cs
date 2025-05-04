using Domain.Entities;
using Domain.Enums;
using Infrastructure.Dtos;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Infrastructure.Dtos.InstagramDto;
using static Infrastructure.Dtos.ProductDto;
using static Infrastructure.Dtos.SharedDto;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    #region Initialization

    private readonly Context _context;
    private readonly InstagramApiKeys _instagramApiKeys;

    public ProductRepository(Context context, IOptions<InstagramApiKeys> instagramApiKeys)
    {
        _context = context;
        _instagramApiKeys = instagramApiKeys.Value;
    }

    #endregion

    #region Methods

    public async Task<long> CreateProduct(ProductCreateDto productCreateDto)
    {
        var product = new Product
        {
            Uuid = Guid.NewGuid(),
            Name = productCreateDto.Name,
            Description = productCreateDto.Description,
            Price = productCreateDto.OriginalPrice - productCreateDto.OriginalPrice * productCreateDto.DiscountPercentage / 100,
            OriginalPrice = productCreateDto.OriginalPrice,
            DiscountPercentage = productCreateDto.DiscountPercentage,
            Stock = productCreateDto.Stock,
            ShopId = productCreateDto.ShopId,
            HasDiscount = productCreateDto.DiscountPercentage > 0,
            IsAvailable = productCreateDto.Stock > 0,
            IsNew = true,
            IsDeleted = false,
            CreateDate = DateTime.UtcNow,
            Images = productCreateDto.Images
        };

        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync();

        var productCategories = productCreateDto.CategoryIds.Select(ci => new ProductCategory
        {
            ProductId = product.Id,
            CategoryId = ci
        });

        await _context.ProductCategory.AddRangeAsync(productCategories);
        await _context.SaveChangesAsync();

        return product.Id;
    }

    public async Task<long> CreateIgProduct(InstagramPostDto instagramPostDto, long shopId, List<int> categoryIds)
    {
        var product = new Product
        {
            Uuid = Guid.NewGuid(),
            IgId = instagramPostDto.Id,
            Name = "",
            Description = instagramPostDto.Caption?.Text,
            Price = 0,
            OriginalPrice = 0,
            DiscountPercentage = 0,
            Stock = 0,
            IgCode = instagramPostDto.Code,
            IgThumbnailSrc = instagramPostDto.ThumbnailSrc,
            IgDisplayUrl = instagramPostDto.DisplayUrl,
            IgLikeCount = instagramPostDto.LikeCount,
            IgCommentCount = instagramPostDto.CommentCount,
            IgCarouselMediaCount = instagramPostDto.CarouselMediaCount,
            IgVideoUrl = instagramPostDto.VideoUrl,
            ShopId = shopId,
            HasDiscount = false,
            IsAvailable = true,
            IsNew = true,
            IsDeleted = false,
            IgIsVideo = instagramPostDto.IsVideo,
            CreateDate = DateTime.UtcNow,
            Images = instagramPostDto.CarouselMedia.Select(cm => cm.ImageUrl).ToList(),
            IgDimensions = instagramPostDto.Dimensions,
            IgCaption = instagramPostDto.Caption,
            IgLocation = instagramPostDto.Location,
        };

        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync();

        if (product.IgCarouselMediaCount is not null)
        {
            var productIgCarouselMediaList = new List<ProductIgCarouselMedia>();
            byte order = 0;
            foreach (var media in instagramPostDto.CarouselMedia)
            {
                var productIgCarouselMedia = new ProductIgCarouselMedia
                {
                    Uuid = Guid.NewGuid(),
                    IgId = media.Id,
                    Code = media.Code,
                    DisplayUrl = media.DisplayUrl,
                    ImageUrl = media.ImageUrl,
                    VideoUrl = media.VideoUrl,
                    Order = order++,
                    ProductId = product.Id,
                    IsVideo = media.IsVideo,
                    Dimensions = media.Dimensions
                };

                productIgCarouselMediaList.Add(productIgCarouselMedia);
            }

            await _context.ProductIgCarouselMedia.AddRangeAsync(productIgCarouselMediaList);
        }
        else
        {
            product.Images = new List<string> { instagramPostDto.DisplayUrl };
        }

        var productCategories = categoryIds.Select(ci => new ProductCategory
        {
            ProductId = product.Id,
            CategoryId = ci
        });

        await _context.ProductCategory.AddRangeAsync(productCategories);
        await _context.SaveChangesAsync();

        return product.Id;
    }

    public async Task<List<long>> CreateIgProducts(List<InstagramPostDto> instagramPostsDto, long shopId, List<int> categoryIds)
    {
        var productIds = new List<long>();
        foreach (var instagramPostDto in instagramPostsDto)
        {
            var existedProduct = await GetProductByIgId(instagramPostDto.Id);

            if (existedProduct is not null)
            {
                continue;
            }

            var product = new Product
            {
                Uuid = Guid.NewGuid(),
                IgId = instagramPostDto.Id,
                Name = "",
                Description = instagramPostDto.Caption?.Text,
                Price = 0,
                OriginalPrice = 0,
                DiscountPercentage = 0,
                Stock = 0,
                IgCode = instagramPostDto.Code,
                IgThumbnailSrc = instagramPostDto.ThumbnailSrc,
                IgDisplayUrl = instagramPostDto.DisplayUrl,
                IgLikeCount = instagramPostDto.LikeCount,
                IgCommentCount = instagramPostDto.CommentCount,
                IgCarouselMediaCount = instagramPostDto.CarouselMediaCount,
                IgVideoUrl = instagramPostDto.VideoUrl,
                ShopId = shopId,
                HasDiscount = false,
                IsAvailable = true,
                IsNew = true,
                IsDeleted = false,
                IgIsVideo = instagramPostDto.IsVideo,
                CreateDate = DateTime.UtcNow,
                Images = instagramPostDto.CarouselMedia is not null ? instagramPostDto.CarouselMedia.Select(cm => cm.DisplayUrl).ToList() : new List<string> { instagramPostDto.DisplayUrl },
                IgDimensions = instagramPostDto.Dimensions,
                IgCaption = instagramPostDto.Caption,
                IgLocation = instagramPostDto.Location,
            };

            var images = new List<string>();
            foreach (var image in product.Images)
            {
                var imageName = Guid.NewGuid().ToString() + ".png";
                var imagePath = $@"{_instagramApiKeys.IgImagesDirectory}\{imageName}";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        using (HttpResponseMessage response = await client.GetAsync(image))
                        {
                            response.EnsureSuccessStatusCode();

                            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                            await File.WriteAllBytesAsync(imagePath, imageBytes);

                            images.Add(imageName);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error downloading image: {ex.Message}");
                    }
                }

                var randomDelay = new Random().Next(500, 3000);
                await Task.Delay(randomDelay);
            }

            product.Images = images;

            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();

            if (product.IgCarouselMediaCount is not null)
            {
                var productIgCarouselMediaList = new List<ProductIgCarouselMedia>();
                byte order = 0;
                foreach (var media in instagramPostDto.CarouselMedia)
                {
                    var productIgCarouselMedia = new ProductIgCarouselMedia
                    {
                        Uuid = Guid.NewGuid(),
                        IgId = media.Id,
                        Code = media.Code,
                        DisplayUrl = media.DisplayUrl,
                        ImageUrl = media.ImageUrl,
                        VideoUrl = media.VideoUrl,
                        Order = order++,
                        ProductId = product.Id,
                        IsVideo = media.IsVideo,
                        Dimensions = media.Dimensions
                    };

                    productIgCarouselMediaList.Add(productIgCarouselMedia);
                }

                await _context.ProductIgCarouselMedia.AddRangeAsync(productIgCarouselMediaList);
            }

            var productCategories = categoryIds.Select(ci => new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = ci
            });

            await _context.ProductCategory.AddRangeAsync(productCategories);
            await _context.SaveChangesAsync();

            productIds.Add(product.Id);
        }

        return productIds;
    }

    public async Task<Product> GetProductById(long productId)
    {
        return await _context.Product.Where(p => p.Id == productId).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).Include(p => p.Shop).FirstOrDefaultAsync();
    }

    public async Task<Product> GetProductByIgId(string productIgId)
    {
        return await _context.Product.FirstOrDefaultAsync(p => p.IgId == productIgId);
    }

    public async Task<List<Product>> GetAllProducts(GetAllDto getAllDto)
    {
        IQueryable<Product> query;

        if (getAllDto.CategoryIds is null || getAllDto.CategoryIds.Count == 0)
        {
            query = _context.Product.Where(p => !p.IsDeleted && p.Name.Contains(getAllDto.SearchTerm));

            if (getAllDto.PageNumber > 0 && getAllDto.PageSize > 0)
            {
                query = query
                    .Skip((getAllDto.PageNumber - 1) * getAllDto.PageSize)
                    .Take(getAllDto.PageSize);
            }

            if (getAllDto.SortType is not null)
            {
                query = SortProducts(query, getAllDto.SortType.Value);
            }

            return await query.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
        }

        var productIdsQuery = _context.Product
            .Join(_context.ProductCategory, p => p.Id, pc => pc.ProductId, (p, pc) => new { p, pc })
            .Where(ppc => !ppc.p.IsDeleted && getAllDto.CategoryIds.Contains(ppc.pc.CategoryId) && ppc.p.Name.Contains(getAllDto.SearchTerm))
            .GroupBy(ppc => ppc.p.Id)
            .Select(g => g.Key);

        if (getAllDto.PageNumber > 0 && getAllDto.PageSize > 0)
        {
            productIdsQuery = productIdsQuery
                .Skip((getAllDto.PageNumber - 1) * getAllDto.PageSize)
                .Take(getAllDto.PageSize);
        }

        var productIds = await productIdsQuery.ToListAsync();
        query = _context.Product.Where(p => productIds.Contains(p.Id));

        if (getAllDto.SortType is not null)
        {
            query = SortProducts(query, getAllDto.SortType.Value);
        }

        return await query.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
    }

    public async Task<List<Product>> GetAllProductsByShopId(long shopId, int pageNumber, int pageSize)
    {
        return await _context.Product.Where(p => !p.IsDeleted && p.ShopId == shopId).Skip((pageNumber - 1) * pageSize).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
    }

    public async Task<List<Product>> GetAllProductsByCategoryIds(List<int> categoryIds)
    {
        return await _context.Product.Join(_context.ProductCategory, p => p.Id, pc => pc.ProductId, (p, pc) => new { p, pc }).Where(ppc => !ppc.p.IsDeleted && categoryIds.Contains(ppc.pc.CategoryId)).Select(ppc => ppc.p).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
    }

    public async Task<List<Product>> GetAllAvailableProducts()
    {
        return await _context.Product.Where(p => !p.IsDeleted && p.IsAvailable).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
    }

    public async Task<long> UpdateProduct(ProductUpdateDto productUpdateDto)
    {
        var product = await GetProductById(productUpdateDto.Id);

        if (product is null)
        {
            throw new InvalidOperationException("Product does not exist.");
        }

        // TODO
        product.Name = productUpdateDto.Name ?? product.Name;
        product.Description = productUpdateDto.Description ?? product.Description;
        product.Price = productUpdateDto.Price ?? product.Price;
        product.DiscountPercentage = productUpdateDto.DiscountPercentage ?? product.DiscountPercentage;
        product.Stock = productUpdateDto.Stock ?? product.Stock;
        product.HasDiscount = product.DiscountPercentage > 0;
        product.IsAvailable = product.Stock > 0;
        product.UpdateDate = DateTime.UtcNow;
        product.Images = productUpdateDto.Images ?? product.Images;

        await _context.SaveChangesAsync();

        return product.Id;
    }

    public async Task<long> DeleteProduct(long productId)
    {
        var product = await GetProductById(productId);

        if (product is null)
        {
            throw new InvalidOperationException("Product does not exist.");
        }

        product.IsDeleted = true;

        await _context.SaveChangesAsync();

        return productId;
    }

    #endregion

    #region Utilities

    private IQueryable<Product> SortProducts(IQueryable<Product> query, SortType sortType)
    {
        switch (sortType)
        {
            case SortType.Featured:
                return query;

            case SortType.NameAsc:
                return query.OrderBy(p => p.Name);

            case SortType.NameDesc:
                return query.OrderByDescending(p => p.Name);

            case SortType.PriceAsc:
                return query.OrderBy(p => p.Price);

            case SortType.PriceDesc:
                return query.OrderByDescending(p => p.Price);

            case SortType.CreateDateAsc:
                return query.OrderBy(p => p.CreateDate);

            case SortType.CreateDateDesc:
                return query.OrderByDescending(p => p.CreateDate);

            default:
                throw new ArgumentOutOfRangeException(nameof(sortType), sortType, null);
        }
    }

    #endregion
}
