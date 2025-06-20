using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class ReceiptDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? ImageUrl { get; set; }
        public List<ReceiptItemDTO> Items { get; set; } = new List<ReceiptItemDTO>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ReceiptItemDTO
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }

    public class CreateReceiptDto
    {
        [Required]
        public int? StoreId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        public string? OcrText { get; set; }

        public string? Items { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }

    public class CreateReceiptItemDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
} 