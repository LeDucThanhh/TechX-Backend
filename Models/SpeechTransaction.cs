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
        [Column("raw_text")]
        public string RawText { get; set; } = string.Empty;

        [Column("processed_text")]
        public string? ProcessedText { get; set; }

        [Column("extracted_data")]
        public string? ExtractedData { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "pending";

        [MaxLength(500)]
        [Column("error_message")]
        public string? ErrorMessage { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("amount")]
        public decimal? Amount { get; set; }

        [MaxLength(100)]
        [Column("category")]
        public string? Category { get; set; }

        [MaxLength(500)]
        [Column("description")]
        public string? Description { get; set; }

        [MaxLength(20)]
        [Column("type")]
        public string? Type { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}