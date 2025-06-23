using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("type")]
        public string Type { get; set; } = string.Empty; // income, expense

        [Required]
        [MaxLength(50)]
        [Column("icon")]
        public string Icon { get; set; } = string.Empty;

        [Required]
        [Column("color")]
        public int Color { get; set; }

        [MaxLength(500)]
        [Column("description")]
        public string? Description { get; set; }

        [Column("is_default")]
        public bool IsDefault { get; set; } = false;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties - no initialization to avoid EF conflicts
        public virtual ICollection<Transaction> Transactions { get; set; } = null!;
        public virtual ICollection<Budget> Budgets { get; set; } = null!;
    }
} 