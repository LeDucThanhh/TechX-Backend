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
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("quantity")]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Required]
        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [MaxLength(100)]
        [Column("category")]
        public string? Category { get; set; }

        [MaxLength(500)]
        [Column("description")]
        public string? Description { get; set; }

        [ForeignKey("ReceiptId")]
        public virtual Receipt Receipt { get; set; } = null!;
    }
} 