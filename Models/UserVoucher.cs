using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("user_vouchers")]
    public class UserVoucher
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

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "available"; // available, used, expired

        [Column("obtained_at")]
        public DateTime ObtainedAt { get; set; } = DateTime.UtcNow;

        [Column("used_at")]
        public DateTime? UsedAt { get; set; }

        [Column("transaction_id")]
        public int? TransactionId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("VoucherId")]
        public virtual Voucher Voucher { get; set; } = null!;

        [ForeignKey("TransactionId")]
        public virtual Transaction? Transaction { get; set; }
    }
} 