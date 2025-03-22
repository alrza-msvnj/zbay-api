using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.Dtos.CategoryDto;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    #region Initialization

    private readonly Context _context;

    public CategoryRepository(Context context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<ushort> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        if (categoryCreateDto.ParentId is not null)
        {
            var parent = await GetCategoryById(categoryCreateDto.ParentId.Value);

            if (parent is null)
            {
                throw new InvalidOperationException("Parent category does not exist.");
            }
        }

        var category = new Category
        {
            Title = categoryCreateDto.Title,
            ParentId = categoryCreateDto.ParentId
        };

        await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync();

        return category.Id;
    }

    public async Task<Category> GetCategoryById(ushort categoryId)
    {
        return await _context.Category.FindAsync(categoryId);
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await _context.Category.ToListAsync();
    }

    #endregion
}
