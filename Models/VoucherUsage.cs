using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("voucher_usage")]
    public class VoucherUsage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("voucher_id")]
        public int VoucherId { get; set; }

        [Column("transaction_id")]
        public int? TransactionId { get; set; }

        [Required]
        [Column("original_amount")]
        public decimal OriginalAmount { get; set; }

        [Required]
        [Column("discount_amount")]
        public decimal DiscountAmount { get; set; }

        [Required]
        [Column("final_amount")]
        public decimal FinalAmount { get; set; }

        [Column("used_at")]
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("VoucherId")]
        public virtual Voucher Voucher { get; set; } = null!;

        [ForeignKey("TransactionId")]
        public virtual Transaction? Transaction { get; set; }
    }
} 