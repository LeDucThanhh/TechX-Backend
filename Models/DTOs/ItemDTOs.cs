using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string? StoreName { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string? Images { get; set; }
        public string? Category { get; set; }
        public string? Tags { get; set; }
        public bool IsAvailable { get; set; }
        public int? StockQuantity { get; set; }
        public decimal? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateItemDTO
    {
        [Required]
        public int StoreId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? OriginalPrice { get; set; }

        [Range(0, 100)]
        public decimal? DiscountPercentage { get; set; }

        public string? Images { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        public string? Tags { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }

        public bool IsFeatured { get; set; } = false;
    }

    public class UpdateItemDTO
    {
        [StringLength(255)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? OriginalPrice { get; set; }

        [Range(0, 100)]
        public decimal? DiscountPercentage { get; set; }

        public string? Images { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        public string? Tags { get; set; }

        public bool? IsAvailable { get; set; }

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }

        public bool? IsFeatured { get; set; }
    }
} 