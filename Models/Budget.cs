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
        
        [Column("category_name")]
        [StringLength(100)]
        public string? CategoryName { get; set; }
        
        [Required]
        [Column("amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // > 0
        
        [Required]
        [Column("period")]
        [StringLength(20)]
        public string Period { get; set; } = string.Empty; // 'daily', 'weekly', 'monthly', 'yearly'
        
        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }
        
        [Column("spent_amount", TypeName = "decimal(18,2)")]
        public decimal SpentAmount { get; set; } = 0;
        
        [Column("remaining_amount", TypeName = "decimal(18,2)")]
        public decimal RemainingAmount { get; set; } // GENERATED ALWAYS AS (amount - spent_amount) STORED
        
        [Column("alerts", TypeName = "jsonb")]
        public string? Alerts { get; set; } // JSON array of BudgetAlert objects
        
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