using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("vouchers")]
    public class Voucher
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("code")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Column("description")]
        public string? Description { get; set; }

        [MaxLength(500)]
        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("type")]
        public string Type { get; set; } = string.Empty; // percentage, fixed_amount, cashback, points

        [Required]
        [Column("value")]
        public decimal Value { get; set; }

        [Column("min_purchase_amount")]
        public decimal MinPurchaseAmount { get; set; } = 0;

        [Column("max_discount_amount")]
        public decimal? MaxDiscountAmount { get; set; }

        [Column("usage_limit")]
        public int? UsageLimit { get; set; }

        [Column("usage_per_user")]
        public int UsagePerUser { get; set; } = 1;

        [Column("current_usage")]
        public int CurrentUsage { get; set; } = 0;

        [Required]
        [Column("valid_from")]
        public DateTime ValidFrom { get; set; }

        [Required]
        [Column("valid_until")]
        public DateTime ValidUntil { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "active"; // active, inactive, expired

        [Column("store_id")]
        public int? StoreId { get; set; }

        [MaxLength(255)]
        [Column("store_name")]
        public string? StoreName { get; set; }

        [Column("applicable_categories")]
        public string? ApplicableCategories { get; set; } // JSONB stored as string

        [Column("terms_conditions")]
        public string? TermsConditions { get; set; }

        [Column("is_featured")]
        public bool IsFeatured { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }
        
        public virtual ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
        public virtual ICollection<VoucherUsage> VoucherUsages { get; set; } = new List<VoucherUsage>();
    }


} 