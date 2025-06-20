using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("loyalty_points")]
    public class LoyaltyPoints
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("store_id")]
        public int? StoreId { get; set; }
        
        [Column("store_name")]
        [StringLength(255)]
        public string? StoreName { get; set; }
        
        [Required]
        [Column("points")]
        public int Points { get; set; } // > 0
        
        [Required]
        [Column("points_value", TypeName = "decimal(18,2)")]
        public decimal PointsValue { get; set; } // >= 0
        
        [Required]
        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }
        
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "active"; // 'active', 'used', 'expired'
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("used_at")]
        public DateTime? UsedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }
    }
} 