using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("stores")]
    public class Store
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Column("description")]
        public string? Description { get; set; }

        [MaxLength(500)]
        [Column("logo")]
        public string? Logo { get; set; }

        [MaxLength(500)]
        [Column("banner")]
        public string? Banner { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [Column("latitude")]
        public decimal? Latitude { get; set; }

        [Column("longitude")]
        public decimal? Longitude { get; set; }

        [MaxLength(20)]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [MaxLength(255)]
        [Column("email")]
        public string? Email { get; set; }

        [MaxLength(500)]
        [Column("website")]
        public string? Website { get; set; }

        [Column("operating_hours")]
        public string? OperatingHours { get; set; } // JSONB stored as string

        [Column("cashback_rate")]
        public decimal CashbackRate { get; set; } = 0;

        [Column("points_rate")]
        public decimal PointsRate { get; set; } = 0;

        [Column("rating")]
        public decimal Rating { get; set; } = 0;

        [Column("review_count")]
        public int ReviewCount { get; set; } = 0;

        [Column("distance")]
        public decimal? Distance { get; set; }

        [Column("is_partner")]
        public bool IsPartner { get; set; } = false;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties - no initialization to avoid EF conflicts
        public virtual ICollection<Transaction> Transactions { get; set; } = null!;
        public virtual ICollection<Item> Items { get; set; } = null!;
        public virtual ICollection<Receipt> Receipts { get; set; } = null!;
        public virtual ICollection<LoyaltyPoint> LoyaltyPoints { get; set; } = null!;
        public virtual ICollection<CashbackTransaction> CashbackTransactions { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = null!;
        public virtual ICollection<Voucher> Vouchers { get; set; } = null!;
    }
} 