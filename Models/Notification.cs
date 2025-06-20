using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("notifications")]
    public class Notification
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Required]
        [Column("title")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [Column("message")]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;
        
        [Required]
        [Column("type")]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // 'transaction', 'budget', 'cashback', 'loyalty', 'general'
        
        [Column("data", TypeName = "jsonb")]
        public string? Data { get; set; } // JSON object for additional data
        
        [Column("is_read")]
        public bool IsRead { get; set; } = false;
        
        [Column("read_at")]
        public DateTime? ReadAt { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
} 