using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("cashback_transactions")]
    public class CashbackTransaction
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("transaction_id")]
        public int? TransactionId { get; set; }
        
        [Column("store_id")]
        public int? StoreId { get; set; }
        
        [Column("store_name")]
        [StringLength(255)]
        public string? StoreName { get; set; }
        
        [Required]
        [Column("transaction_amount", TypeName = "decimal(18,2)")]
        public decimal TransactionAmount { get; set; }
        
        [Required]
        [Column("cashback_amount", TypeName = "decimal(18,2)")]
        public decimal CashbackAmount { get; set; }
        
        [Required]
        [Column("cashback_rate", TypeName = "decimal(5,2)")]
        public decimal CashbackRate { get; set; }
        
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "pending"; // 'pending', 'approved', 'paid', 'rejected'
        
        [Required]
        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }
        
        [Column("approved_date")]
        public DateTime? ApprovedDate { get; set; }
        
        [Column("paid_date")]
        public DateTime? PaidDate { get; set; }
        
        [Column("notes")]
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        [Column("rejection_reason")]
        [StringLength(500)]
        public string? RejectionReason { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        [ForeignKey("TransactionId")]
        public virtual Transaction? Transaction { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }
    }
} 