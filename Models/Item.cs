using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("items")]
    public class Item
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("store_id")]
        public int StoreId { get; set; }
        
        [Column("store_name")]
        [StringLength(255)]
        public string? StoreName { get; set; }
        
        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [Column("description")]
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // >= 0
        
        [Column("original_price", TypeName = "decimal(18,2)")]
        public decimal? OriginalPrice { get; set; }
        
        [Column("discount_percentage", TypeName = "decimal(5,2)")]
        public decimal? DiscountPercentage { get; set; } // 0-100%
        
        [Column("images", TypeName = "jsonb")]
        public string? Images { get; set; } // JSON array of strings
        
        [Column("category")]
        [StringLength(100)]
        public string? Category { get; set; }
        
        [Column("tags", TypeName = "jsonb")]
        public string? Tags { get; set; } // JSON array of strings
        
        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;
        
        [Column("stock_quantity")]
        public int? StockQuantity { get; set; }
        
        [Column("rating", TypeName = "decimal(3,2)")]
        public decimal? Rating { get; set; } // 0-5
        
        [Column("review_count")]
        public int? ReviewCount { get; set; }
        
        [Column("is_featured")]
        public bool IsFeatured { get; set; } = false;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Store Store { get; set; } = null!;
    }
} 