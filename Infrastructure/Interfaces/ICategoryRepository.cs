using Domain.Entities;

namespace Application.Interfaces;

public interface ICategoryRepository
{
    Task<ushort> CreateCategory(CategoryCreateDto categoryCreateDto);
    Task<Category> GetCategoryById(ushort categoryId);
    Task<List<Category>> GetAllCategories();
}
