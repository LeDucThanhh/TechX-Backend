using TechX.API.Models.DTOs;

namespace TechX.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO createDto);
        Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO updateDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
} 