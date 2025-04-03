using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Infrastructure.Dtos.ProductDto;
using static Infrastructure.Dtos.SharedDto;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    #region Initialization

    private readonly Context _context;

    public ProductRepository(Context context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<ulong> CreateProduct(ProductCreateDto productCreateDto)
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
            IsAvailable = productCreateDto.IsAvailable,
            IsNew = true,
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

    public async Task<Product> GetProductById(ulong productId)
    {
        return await _context.Product.Where(p => p.Id == productId).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).Include(p => p.Shop).FirstOrDefaultAsync();
    }

    public async Task<List<Product>> GetAllProducts(GetAllDto getAllDto)
    {
        if (getAllDto.CategoryIds is null || getAllDto.CategoryIds.IsNullOrEmpty())
        {
            var query = _context.Product.Where(p => !p.IsDeleted);

            if (getAllDto.PageNumber > 0 && getAllDto.PageSize > 0)
            {
                query = query
                    .Skip((getAllDto.PageNumber - 1) * getAllDto.PageSize)
                    .Take(getAllDto.PageSize);
            }

            return await query.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
        }

        var productIdsQuery = _context.Product
            .Join(_context.ProductCategory, p => p.Id, pc => pc.ProductId, (p, pc) => new { p, pc })
            .Where(ppc => !ppc.p.IsDeleted && getAllDto.CategoryIds.Contains(ppc.pc.CategoryId))
            .GroupBy(ppc => ppc.p.Id)
            .Select(g => g.Key);

        if (getAllDto.PageNumber > 0 && getAllDto.PageSize > 0)
        {
            productIdsQuery = productIdsQuery
                .Skip((getAllDto.PageNumber - 1) * getAllDto.PageSize)
                .Take(getAllDto.PageSize);
        }

        var productIds = await productIdsQuery.ToListAsync();

        return await _context.Product
            .Where(p => productIds.Contains(p.Id))
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .ToListAsync();
    }

    public async Task<List<Product>> GetAllProductsByShopId(uint shopId, ushort pageNumber, ushort pageSize)
    {
        return await _context.Product.Where(p => !p.IsDeleted && p.ShopId == shopId).Skip((pageNumber - 1) * pageSize).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
    }

    public async Task<List<Product>> GetAllProductsByCategoryIds(List<ushort> categoryIds)
    {
        return await _context.Product.Join(_context.ProductCategory, p => p.Id, pc => pc.ProductId, (p, pc) => new { p, pc }).Where(ppc => !ppc.p.IsDeleted && categoryIds.Contains(ppc.pc.CategoryId)).Select(ppc => ppc.p).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
    }

    public async Task<List<Product>> GetAllAvailableProducts()
    {
        return await _context.Product.Where(p => !p.IsDeleted && p.IsAvailable).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).ToListAsync();
    }

    public async Task<ulong> UpdateProduct(ProductUpdateDto productUpdateDto)
    {
        var product = await GetProductById(productUpdateDto.Id);

        if (product is null)
        {
            throw new InvalidOperationException("Product does not exist.");
        }

        product.Name = productUpdateDto.Name ?? product.Name;
        product.Description = productUpdateDto.Description ?? product.Description;
        product.Price = productUpdateDto.Price ?? product.Price;
        product.DiscountPercentage = productUpdateDto.DiscountPercentage ?? product.DiscountPercentage;
        product.Stock = productUpdateDto.Stock ?? product.Stock;
        product.HasDiscount = product.DiscountPercentage > 0;
        product.IsAvailable = productUpdateDto.IsAvailable ?? product.IsAvailable;
        product.UpdateDate = DateTime.UtcNow;
        product.Images = productUpdateDto.Images ?? product.Images;

        await _context.SaveChangesAsync();

        return product.Id;
    }

    public async Task<ulong> DeleteProduct(ulong productId)
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
}
