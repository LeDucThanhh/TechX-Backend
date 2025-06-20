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
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Column("type")]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty; // 'income' or 'expense'
        
        [Required]
        [Column("icon")]
        [StringLength(50)]
        public string Icon { get; set; } = string.Empty;
        
        [Required]
        [Column("color")]
        public int Color { get; set; }
        
        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Column("is_default")]
        public bool IsDefault { get; set; } = false;
        
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    }
} 