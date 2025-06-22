using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("transactions")]
    public class Transaction
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("type")]
        public string Type { get; set; } = string.Empty; // income, expense

        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [MaxLength(100)]
        [Column("category_name")]
        public string? CategoryName { get; set; }

        [MaxLength(500)]
        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

        [Column("store_id")]
        public int? StoreId { get; set; }

        [MaxLength(255)]
        [Column("store_name")]
        public string? StoreName { get; set; }

        [MaxLength(500)]
        [Column("receipt_url")]
        public string? ReceiptUrl { get; set; }

        [Column("tags")]
        public string? Tags { get; set; } // JSONB stored as string

        [Column("is_recurring")]
        public bool IsRecurring { get; set; } = false;

        [MaxLength(20)]
        [Column("recurring_type")]
        public string? RecurringType { get; set; }

        [Column("cashback_amount")]
        public decimal? CashbackAmount { get; set; }

        [Column("points_earned")]
        public int? PointsEarned { get; set; }

        [MaxLength(1000)]
        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;

        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }

        public virtual ICollection<CashbackTransaction> CashbackTransactions { get; set; } = new List<CashbackTransaction>();
        public virtual ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
        public virtual ICollection<VoucherUsage> VoucherUsages { get; set; } = new List<VoucherUsage>();
    }
} 