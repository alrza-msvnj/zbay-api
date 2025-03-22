using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Infrastructure.Dtos.CategoryDto;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    #region Initialization

    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    #endregion

    #region Apis

    [HttpPost(nameof(CreateCategory))]
    public async Task<IActionResult> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        var categoryId = await _categoryRepository.CreateCategory(categoryCreateDto);

        return Ok(categoryId);
    }

    [HttpGet($"{nameof(GetCategoryById)}/{{categoryId}}")]
    public async Task<IActionResult> GetCategoryById(ushort categoryId)
    {
        var category = await _categoryRepository.GetCategoryById(categoryId);

        return Ok(category);
    }

    [HttpGet(nameof(GetAllCategories))]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllCategories();

        return Ok(categories);
    }

    #endregion
}
