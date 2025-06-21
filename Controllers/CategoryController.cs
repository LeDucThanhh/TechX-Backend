using Microsoft.AspNetCore.Mvc;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            try
            {
                _logger.LogInformation("Getting all categories");
                var categories = await _categoryService.GetAllCategoriesAsync();
                _logger.LogInformation("Retrieved {Count} categories", categories.Count());
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories");
                return StatusCode(500, new { 
                    message = "An error occurred while retrieving categories",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving category" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CreateCategoryDTO createDto)
        {
            try
            {
                var category = await _categoryService.CreateCategoryAsync(createDto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while creating category" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, [FromBody] UpdateCategoryDTO updateDto)
        {
            try
            {
                var category = await _categoryService.UpdateCategoryAsync(id, updateDto);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating category" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Category not found" });
                }
                return Ok(new { message = "Category deleted successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while deleting category" });
            }
        }
    }
} 