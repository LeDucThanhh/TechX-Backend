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

        [MaxLength(255)]
        [Column("store_name")]
        public string? StoreName { get; set; }

        [Required]
        [Column("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [Required]
        [Column("cashback_amount")]
        public decimal CashbackAmount { get; set; }

        [Required]
        [Column("cashback_rate")]
        public decimal CashbackRate { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "pending";

        [Required]
        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [Column("approved_date")]
        public DateTime? ApprovedDate { get; set; }

        [Column("paid_date")]
        public DateTime? PaidDate { get; set; }

        [MaxLength(1000)]
        [Column("notes")]
        public string? Notes { get; set; }

        [MaxLength(500)]
        [Column("rejection_reason")]
        public string? RejectionReason { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("TransactionId")]
        public virtual Transaction? Transaction { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }
    }
} 