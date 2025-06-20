using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechX.API.Models
{
    [Table("receipt_items")]
    public class ReceiptItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Required]
        [Column("receipt_id")]
        public int ReceiptId { get; set; }
        
        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [Column("quantity")]
        public int Quantity { get; set; } = 1;
        
        [Required]
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        [Required]
        [Column("total_price", TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
        [Column("category")]
        [StringLength(100)]
        public string? Category { get; set; }
        
        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }
        
        // Navigation properties
        public virtual Receipt Receipt { get; set; } = null!;
    }
} 