using TechX.API.Data;
using TechX.API.Models;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TechX.API.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Icon = c.Icon,
                Color = c.Color.ToString(),
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var c = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) throw new KeyNotFoundException($"Category {id} not found");
            return new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Icon = c.Icon,
                Color = c.Color.ToString(),
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            };
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO createDto)
        {
            var c = new Category
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Icon = createDto.Icon ?? "default_icon",
                Color = int.TryParse(createDto.Color, out var color) ? color : 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Categories.Add(c);
            await _context.SaveChangesAsync();
            return await GetCategoryByIdAsync(c.Id);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO updateDto)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c == null) throw new KeyNotFoundException($"Category {id} not found");
            c.Name = updateDto.Name ?? c.Name;
            c.Description = updateDto.Description ?? c.Description;
            c.Icon = updateDto.Icon ?? c.Icon;
            if (!string.IsNullOrEmpty(updateDto.Color))
                c.Color = int.TryParse(updateDto.Color, out var color) ? color : c.Color;
            c.IsActive = updateDto.IsActive ?? c.IsActive;
            c.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return await GetCategoryByIdAsync(c.Id);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c == null) return false;
            _context.Categories.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 