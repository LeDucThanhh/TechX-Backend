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
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [Column("description")]
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Column("logo")]
        [StringLength(500)]
        public string? Logo { get; set; }
        
        [Column("banner")]
        [StringLength(500)]
        public string? Banner { get; set; }
        
        [Required]
        [Column("address")]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
        
        [Column("latitude", TypeName = "decimal(10,8)")]
        public decimal? Latitude { get; set; }
        
        [Column("longitude", TypeName = "decimal(11,8)")]
        public decimal? Longitude { get; set; }
        
        [Column("phone_number")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [Column("email")]
        [StringLength(255)]
        public string? Email { get; set; }
        
        [Column("website")]
        [StringLength(500)]
        public string? Website { get; set; }
        
        [Column("operating_hours", TypeName = "jsonb")]
        public string? OperatingHours { get; set; } // JSON object for Map<String, String>
        
        [Column("cashback_rate", TypeName = "decimal(5,2)")]
        public decimal CashbackRate { get; set; } = 0; // 0-100%
        
        [Column("points_rate", TypeName = "decimal(5,2)")]
        public decimal PointsRate { get; set; } = 0; // >= 0
        
        [Column("rating", TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; } = 0; // 0-5
        
        [Column("review_count")]
        public int ReviewCount { get; set; } = 0;
        
        [Column("distance", TypeName = "decimal(10,2)")]
        public decimal? Distance { get; set; } // Calculated field for nearby stores
        
        [Column("is_partner")]
        public bool IsPartner { get; set; } = false;
        
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
        public virtual ICollection<CashbackTransaction> CashbackTransactions { get; set; } = new List<CashbackTransaction>();
        public virtual ICollection<LoyaltyPoints> LoyaltyPoints { get; set; } = new List<LoyaltyPoints>();
    }
} 