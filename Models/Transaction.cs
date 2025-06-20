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
        [Column("amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // > 0
        
        [Required]
        [Column("type")]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty; // 'income' or 'expense'
        
        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }
        
        [Column("category_name")]
        [StringLength(100)]
        public string? CategoryName { get; set; }
        
        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [Column("date")]
        public DateTime Date { get; set; }
        
        [Column("store_id")]
        public int? StoreId { get; set; }
        
        [Column("store_name")]
        [StringLength(255)]
        public string? StoreName { get; set; }
        
        [Column("receipt_url")]
        [StringLength(500)]
        public string? ReceiptUrl { get; set; }
        
        [Column("tags", TypeName = "jsonb")]
        public string? Tags { get; set; } // JSON array of strings
        
        [Column("is_recurring")]
        public bool IsRecurring { get; set; } = false;
        
        [Column("recurring_type")]
        [StringLength(20)]
        public string? RecurringType { get; set; } // 'daily', 'weekly', 'monthly', 'yearly'
        
        [Column("cashback_amount", TypeName = "decimal(18,2)")]
        public decimal? CashbackAmount { get; set; } // >= 0
        
        [Column("points_earned")]
        public int? PointsEarned { get; set; } // >= 0
        
        [Column("notes")]
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Store? Store { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual CashbackTransaction? CashbackTransaction { get; set; }
    }
} 