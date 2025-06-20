using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("receipts")]
    public class Receipt
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("store_id")]
        public int? StoreId { get; set; }
        
        [Column("store_name")]
        [StringLength(255)]
        public string? StoreName { get; set; }
        
        [Required]
        [Column("total_amount", TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } // >= 0
        
        [Required]
        [Column("purchase_date")]
        public DateTime PurchaseDate { get; set; }
        
        [Required]
        [Column("image_url")]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;
        
        [Column("ocr_text")]
        public string? OcrText { get; set; }
        
        [Column("items", TypeName = "jsonb")]
        public string? Items { get; set; } // JSON array of ReceiptItem objects
        
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "pending"; // 'pending', 'processed', 'failed'
        
        [Column("cashback_amount", TypeName = "decimal(18,2)")]
        public decimal? CashbackAmount { get; set; } // >= 0
        
        [Column("points_earned")]
        public int? PointsEarned { get; set; } // >= 0
        
        [Column("notes")]
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("processed_at")]
        public DateTime? ProcessedAt { get; set; }
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }
    }
} 