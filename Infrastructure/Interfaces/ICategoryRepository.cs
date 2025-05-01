using Domain.Entities;
using static Infrastructure.Dtos.CategoryDto;

namespace Application.Interfaces;

public interface ICategoryRepository
{
    Task<int> CreateCategory(CategoryCreateDto categoryCreateDto);
    Task<Category> GetCategoryById(int categoryId);
    Task<List<Category>> GetAllCategories();
}
