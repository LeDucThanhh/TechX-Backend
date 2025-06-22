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

        [MaxLength(255)]
        [Column("store_name")]
        public string? StoreName { get; set; }

        [Required]
        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column("purchase_date")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [Column("ocr_text")]
        public string? OcrText { get; set; }

        [Column("items")]
        public string? Items { get; set; } // JSONB

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "pending"; // pending, processed, failed

        [Column("cashback_amount")]
        public decimal? CashbackAmount { get; set; }

        [Column("points_earned")]
        public int? PointsEarned { get; set; }

        [MaxLength(1000)]
        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("processed_at")]
        public DateTime? ProcessedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }

        public virtual ICollection<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();
    }
} 