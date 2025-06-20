using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateReviewDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int StoreId { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
} 