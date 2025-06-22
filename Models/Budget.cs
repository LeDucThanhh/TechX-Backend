using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("budgets")]
    public class Budget
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [MaxLength(100)]
        [Column("category_name")]
        public string? CategoryName { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("period")]
        public string Period { get; set; } = string.Empty; // daily, weekly, monthly, yearly

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("spent_amount")]
        public decimal SpentAmount { get; set; } = 0;

        // Note: remaining_amount is a computed column in database, so we make it read-only
        [Column("remaining_amount")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal RemainingAmount { get; private set; }

        [Column("alerts")]
        public string? Alerts { get; set; } // JSONB stored as string

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;
    }
} 