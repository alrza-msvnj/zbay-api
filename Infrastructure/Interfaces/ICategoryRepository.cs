using Domain.Entities;
using static Infrastructure.Dtos.CategoryDto;

namespace Application.Interfaces;

public interface ICategoryRepository
{
    Task<ushort> CreateCategory(CategoryCreateDto categoryCreateDto);
    Task<Category> GetCategoryById(ushort categoryId);
    Task<List<Category>> GetAllCategories();
}
