using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("reviews")]
    public class Review
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Required]
        [Column("store_id")]
        public int StoreId { get; set; }
        
        [Required]
        [Column("rating")]
        public int Rating { get; set; } // 1-5
        
        [Column("title")]
        [StringLength(200)]
        public string? Title { get; set; }
        
        [Column("comment")]
        [StringLength(1000)]
        public string? Comment { get; set; }
        
        [Column("images", TypeName = "jsonb")]
        public string? Images { get; set; } // JSON array of strings
        
        [Column("is_verified_purchase")]
        public bool IsVerifiedPurchase { get; set; } = false;
        
        [Column("helpful_count")]
        public int HelpfulCount { get; set; } = 0;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Store? Store { get; set; }
    }
} 