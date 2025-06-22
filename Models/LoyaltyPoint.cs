using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("loyalty_points")]
    public class LoyaltyPoint
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
        [Column("points")]
        public int Points { get; set; }

        [Required]
        [Column("points_value")]
        public decimal PointsValue { get; set; }

        [Required]
        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "active";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("used_at")]
        public DateTime? UsedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }
    }
} 