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

        [MaxLength(255)]
        [Column("store_name")]
        public string? StoreName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Column("original_price")]
        public decimal? OriginalPrice { get; set; }

        [Column("discount_percentage")]
        public decimal? DiscountPercentage { get; set; }

        [Column("images")]
        public string? Images { get; set; } // JSONB

        [MaxLength(100)]
        [Column("category")]
        public string? Category { get; set; }

        [Column("tags")]
        public string? Tags { get; set; } // JSONB

        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;

        [Column("stock_quantity")]
        public int? StockQuantity { get; set; }

        [Column("rating")]
        public decimal? Rating { get; set; }

        [Column("review_count")]
        public int? ReviewCount { get; set; }

        [Column("is_featured")]
        public bool IsFeatured { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; } = null!;
    }
} 