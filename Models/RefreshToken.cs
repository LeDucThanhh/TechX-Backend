using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("refresh_tokens")]
    public class RefreshToken
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Required]
        [Column("token")]
        [StringLength(500)]
        public string Token { get; set; } = string.Empty;
        
        [Required]
        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }
        
        [Column("is_revoked")]
        public bool IsRevoked { get; set; } = false;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
} 