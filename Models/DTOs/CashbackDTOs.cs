using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class CashbackTransactionDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? TransactionId { get; set; }
        public int? StoreId { get; set; }
        public string? StoreName { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal CashbackAmount { get; set; }
        public decimal CashbackRate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? Notes { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCashbackTransactionDTO : IValidatableObject
    {
        [Required(ErrorMessage = "User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
        public int UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Transaction ID must be greater than 0")]
        public int? TransactionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int? StoreId { get; set; }

        [StringLength(255, ErrorMessage = "Store name cannot exceed 255 characters")]
        public string? StoreName { get; set; }

        [Required(ErrorMessage = "Transaction amount is required")]
        [Range(0.01, 999999999.99, ErrorMessage = "Transaction amount must be between 0.01 and 999,999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Transaction amount can have maximum 2 decimal places")]
        public decimal TransactionAmount { get; set; }

        [Required(ErrorMessage = "Cashback amount is required")]
        [Range(0, 999999999.99, ErrorMessage = "Cashback amount must be between 0 and 999,999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cashback amount can have maximum 2 decimal places")]
        public decimal CashbackAmount { get; set; }

        [Required(ErrorMessage = "Cashback rate is required")]
        [Range(0, 100, ErrorMessage = "Cashback rate must be between 0 and 100")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cashback rate can have maximum 2 decimal places")]
        public decimal CashbackRate { get; set; }

        [Required(ErrorMessage = "Transaction date is required")]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; }

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Date validation
            if (TransactionDate > DateTime.Now.AddDays(1))
            {
                yield return new ValidationResult("Transaction date cannot be more than 1 day in the future", new[] { nameof(TransactionDate) });
            }
            
            if (TransactionDate < DateTime.Now.AddYears(-2))
            {
                yield return new ValidationResult("Transaction date cannot be more than 2 years in the past", new[] { nameof(TransactionDate) });
            }

            // Cashback calculation validation
            var expectedCashback = TransactionAmount * (CashbackRate / 100);
            var tolerance = 0.01m; // 1 cent tolerance for rounding
            
            if (Math.Abs(CashbackAmount - expectedCashback) > tolerance)
            {
                yield return new ValidationResult($"Cashback amount ({CashbackAmount:F2}) does not match expected amount ({expectedCashback:F2}) based on rate ({CashbackRate}%)", 
                    new[] { nameof(CashbackAmount) });
            }

            // Store or StoreName required
            if (!StoreId.HasValue && string.IsNullOrEmpty(StoreName))
            {
                yield return new ValidationResult("Either Store ID or Store Name must be provided", 
                    new[] { nameof(StoreId), nameof(StoreName) });
            }
        }
    }

    public class UpdateCashbackTransactionDTO : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int? StoreId { get; set; }

        [StringLength(255, ErrorMessage = "Store name cannot exceed 255 characters")]
        public string? StoreName { get; set; }

        [Range(0.01, 999999999.99, ErrorMessage = "Transaction amount must be between 0.01 and 999,999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Transaction amount can have maximum 2 decimal places")]
        public decimal? TransactionAmount { get; set; }

        [Range(0, 999999999.99, ErrorMessage = "Cashback amount must be between 0 and 999,999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cashback amount can have maximum 2 decimal places")]
        public decimal? CashbackAmount { get; set; }

        [Range(0, 100, ErrorMessage = "Cashback rate must be between 0 and 100")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cashback rate can have maximum 2 decimal places")]
        public decimal? CashbackRate { get; set; }

        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        [RegularExpression(@"^(pending|approved|rejected|paid)$", ErrorMessage = "Status must be pending, approved, rejected, or paid")]
        public string? Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TransactionDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ApprovedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? PaidDate { get; set; }

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }

        [StringLength(500, ErrorMessage = "Rejection reason cannot exceed 500 characters")]
        public string? RejectionReason { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Date validation
            if (TransactionDate.HasValue && TransactionDate.Value > DateTime.Now.AddDays(1))
            {
                yield return new ValidationResult("Transaction date cannot be more than 1 day in the future", new[] { nameof(TransactionDate) });
            }

            // Status-specific validations
            if (Status == "approved" && !ApprovedDate.HasValue)
            {
                yield return new ValidationResult("Approved date is required when status is approved", new[] { nameof(ApprovedDate) });
            }

            if (Status == "paid" && !PaidDate.HasValue)
            {
                yield return new ValidationResult("Paid date is required when status is paid", new[] { nameof(PaidDate) });
            }

            if (Status == "rejected" && string.IsNullOrEmpty(RejectionReason))
            {
                yield return new ValidationResult("Rejection reason is required when status is rejected", new[] { nameof(RejectionReason) });
            }

            // Date sequence validation
            if (ApprovedDate.HasValue && TransactionDate.HasValue && ApprovedDate.Value < TransactionDate.Value)
            {
                yield return new ValidationResult("Approved date cannot be before transaction date", new[] { nameof(ApprovedDate) });
            }

            if (PaidDate.HasValue && ApprovedDate.HasValue && PaidDate.Value < ApprovedDate.Value)
            {
                yield return new ValidationResult("Paid date cannot be before approved date", new[] { nameof(PaidDate) });
            }

            // Cashback calculation validation
            if (TransactionAmount.HasValue && CashbackAmount.HasValue && CashbackRate.HasValue)
            {
                var expectedCashback = TransactionAmount.Value * (CashbackRate.Value / 100);
                var tolerance = 0.01m;
                
                if (Math.Abs(CashbackAmount.Value - expectedCashback) > tolerance)
                {
                    yield return new ValidationResult($"Cashback amount does not match expected amount based on rate", 
                        new[] { nameof(CashbackAmount) });
                }
            }
        }
    }

    public class CashbackSummaryDTO
    {
        public decimal TotalCashback { get; set; }
        public decimal PendingCashback { get; set; }
        public decimal ApprovedCashback { get; set; }
        public decimal PaidCashback { get; set; }
        public int TotalTransactions { get; set; }
    }
}