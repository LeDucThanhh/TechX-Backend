using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class SpeechTransactionDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RawText { get; set; } = string.Empty;
        public string? ProcessedText { get; set; }
        public string? ExtractedData { get; set; }
        public string Status { get; set; } = string.Empty; // pending, processed, failed
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal? Amount { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; } // income, expense
        public DateTime? TransactionDate { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateSpeechTransactionDTO : IValidatableObject
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string RawText { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(RawText))
            {
                results.Add(new ValidationResult("Raw text cannot be empty", new[] { nameof(RawText) }));
            }

            if (RawText?.Length < 3)
            {
                results.Add(new ValidationResult("Raw text must be at least 3 characters long", new[] { nameof(RawText) }));
            }

            return results;
        }
    }

    public class ProcessSpeechTransactionDTO : IValidatableObject
    {
        [MaxLength(2000)]
        public string? ProcessedText { get; set; }

        public string? ExtractedData { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Amount { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(20)]
        public string? Type { get; set; }

        public DateTime? TransactionDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!string.IsNullOrEmpty(Type))
            {
                var validTypes = new[] { "income", "expense" };
                if (!validTypes.Contains(Type.ToLower()))
                {
                    results.Add(new ValidationResult("Type must be either 'income' or 'expense'", new[] { nameof(Type) }));
                }
            }

            if (TransactionDate.HasValue && TransactionDate.Value > DateTime.UtcNow)
            {
                results.Add(new ValidationResult("Transaction date cannot be in the future", new[] { nameof(TransactionDate) }));
            }

            return results;
        }
    }

    public class SpeechTransactionSummaryDTO
    {
        public int TotalRequests { get; set; }
        public int ProcessedSuccessfully { get; set; }
        public int Failed { get; set; }
        public int Pending { get; set; }
        public decimal SuccessRate { get; set; }
        public List<SpeechTransactionDTO> RecentTransactions { get; set; } = new List<SpeechTransactionDTO>();
    }
} 