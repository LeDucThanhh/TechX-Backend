using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateCategoryDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s&\-_]+$", ErrorMessage = "Category name can only contain letters, numbers, spaces, &, -, and _")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [StringLength(50, ErrorMessage = "Icon name cannot exceed 50 characters")]
        [RegularExpression(@"^[a-z_]+$", ErrorMessage = "Icon name can only contain lowercase letters and underscores")]
        public string? Icon { get; set; }
        
        [StringLength(20, ErrorMessage = "Color cannot exceed 20 characters")]
        [RegularExpression(@"^(#[0-9A-Fa-f]{6}|#[0-9A-Fa-f]{3}|\d+)$", ErrorMessage = "Color must be a valid hex color (#RRGGBB, #RGB) or numeric value")]
        public string? Color { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Parent category ID must be greater than 0")]
        public int? ParentCategoryId { get; set; }
        
        [Required(ErrorMessage = "Category type is required")]
        [StringLength(20, ErrorMessage = "Type cannot exceed 20 characters")]
        [RegularExpression(@"^(income|expense)$", ErrorMessage = "Type must be 'income' or 'expense'")]
        public string Type { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validate that parent category is not the same as current (for updates)
            if (ParentCategoryId.HasValue && ParentCategoryId.Value == 0)
            {
                yield return new ValidationResult("Invalid parent category ID", new[] { nameof(ParentCategoryId) });
            }
        }
    }

    public class UpdateCategoryDTO : IValidatableObject
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s&\-_]+$", ErrorMessage = "Category name can only contain letters, numbers, spaces, &, -, and _")]
        public string? Name { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [StringLength(50, ErrorMessage = "Icon name cannot exceed 50 characters")]
        [RegularExpression(@"^[a-z_]+$", ErrorMessage = "Icon name can only contain lowercase letters and underscores")]
        public string? Icon { get; set; }
        
        [StringLength(20, ErrorMessage = "Color cannot exceed 20 characters")]
        [RegularExpression(@"^(#[0-9A-Fa-f]{6}|#[0-9A-Fa-f]{3}|\d+)$", ErrorMessage = "Color must be a valid hex color (#RRGGBB, #RGB) or numeric value")]
        public string? Color { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Parent category ID must be greater than 0")]
        public int? ParentCategoryId { get; set; }
        
        public bool? IsActive { get; set; }
        
        [StringLength(20, ErrorMessage = "Type cannot exceed 20 characters")]
        [RegularExpression(@"^(income|expense)$", ErrorMessage = "Type must be 'income' or 'expense'")]
        public string? Type { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validate that parent category is not the same as current (for updates)
            if (ParentCategoryId.HasValue && ParentCategoryId.Value == 0)
            {
                yield return new ValidationResult("Invalid parent category ID", new[] { nameof(ParentCategoryId) });
            }
        }
    }
} 
        
        [Required(ErrorMessage = "Category type is required")]
        [StringLength(20, ErrorMessage = "Type cannot exceed 20 characters")]
        [RegularExpression(@"^(income|expense)$", ErrorMessage = "Type must be 'income' or 'expense'")]
        public string Type { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validate that parent category is not the same as current (for updates)
            if (ParentCategoryId.HasValue && ParentCategoryId.Value == 0)
            {
                yield return new ValidationResult("Invalid parent category ID", new[] { nameof(ParentCategoryId) });
            }
        }
    }

    public class UpdateCategoryDTO : IValidatableObject
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s&\-_]+$", ErrorMessage = "Category name can only contain letters, numbers, spaces, &, -, and _")]
        public string? Name { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [StringLength(50, ErrorMessage = "Icon name cannot exceed 50 characters")]
        [RegularExpression(@"^[a-z_]+$", ErrorMessage = "Icon name can only contain lowercase letters and underscores")]
        public string? Icon { get; set; }
        
        [StringLength(20, ErrorMessage = "Color cannot exceed 20 characters")]
        [RegularExpression(@"^(#[0-9A-Fa-f]{6}|#[0-9A-Fa-f]{3}|\d+)$", ErrorMessage = "Color must be a valid hex color (#RRGGBB, #RGB) or numeric value")]
        public string? Color { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Parent category ID must be greater than 0")]
        public int? ParentCategoryId { get; set; }
        
        public bool? IsActive { get; set; }
        
        [StringLength(20, ErrorMessage = "Type cannot exceed 20 characters")]
        [RegularExpression(@"^(income|expense)$", ErrorMessage = "Type must be 'income' or 'expense'")]
        public string? Type { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validate that parent category is not the same as current (for updates)
            if (ParentCategoryId.HasValue && ParentCategoryId.Value == 0)
            {
                yield return new ValidationResult("Invalid parent category ID", new[] { nameof(ParentCategoryId) });
            }
        }
    }
} 