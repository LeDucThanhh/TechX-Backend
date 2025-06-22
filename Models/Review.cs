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

        [Column("store_id")]
        public int? StoreId { get; set; }

        [MaxLength(255)]
        [Column("user_name")]
        public string? UserName { get; set; }

        [MaxLength(500)]
        [Column("user_avatar")]
        public string? UserAvatar { get; set; }

        [Required]
        [Column("rating")]
        public decimal Rating { get; set; }

        [MaxLength(1000)]
        [Column("comment")]
        public string? Comment { get; set; }

        [Column("images")]
        public string? Images { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("is_verified")]
        public bool IsVerified { get; set; } = false;

        [Column("helpful_count")]
        public int HelpfulCount { get; set; } = 0;

        [Column("is_helpful")]
        public bool IsHelpful { get; set; } = false;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }
    }
}