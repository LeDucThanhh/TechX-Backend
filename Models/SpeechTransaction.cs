using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("speech_transactions")]
    public class SpeechTransaction
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Required]
        [Column("voice_text")]
        [StringLength(1000)]
        public string VoiceText { get; set; } = string.Empty;
        
        [Required]
        [Column("language")]
        [StringLength(10)]
        public string Language { get; set; } = "vi"; // 'vi', 'en'
        
        [Column("extracted_amount", TypeName = "decimal(18,2)")]
        public decimal? ExtractedAmount { get; set; }
        
        [Column("extracted_category")]
        [StringLength(100)]
        public string? ExtractedCategory { get; set; }
        
        [Column("extracted_description")]
        [StringLength(500)]
        public string? ExtractedDescription { get; set; }
        
        [Column("confidence_score")]
        public decimal? ConfidenceScore { get; set; } // 0-1
        
        [Column("processing_time_ms")]
        public int? ProcessingTimeMs { get; set; }
        
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "pending"; // 'pending', 'processed', 'failed'
        
        [Column("error_message")]
        [StringLength(500)]
        public string? ErrorMessage { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("processed_at")]
        public DateTime? ProcessedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
} 