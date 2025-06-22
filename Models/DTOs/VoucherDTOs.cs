using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class VoucherDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string Type { get; set; } = string.Empty; // percentage, fixed_amount, cashback, points
        public decimal Value { get; set; }
        public decimal MinPurchaseAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public int? UsageLimit { get; set; }
        public int UsagePerUser { get; set; }
        public int CurrentUsage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public string Status { get; set; } = string.Empty; // active, inactive, expired
        public int? StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? ApplicableCategories { get; set; }
        public string? TermsConditions { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCollected { get; set; } // For user context
        public bool IsUsed { get; set; } // For user context
        public bool IsExpired => DateTime.UtcNow > ValidUntil;
        public bool IsActive => Status == "active" && !IsExpired;
    }

    public class CreateVoucherDTO : IValidatableObject
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Value { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MinPurchaseAmount { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal? MaxDiscountAmount { get; set; }

        [Range(1, int.MaxValue)]
        public int? UsageLimit { get; set; }

        [Range(1, int.MaxValue)]
        public int UsagePerUser { get; set; } = 1;

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidUntil { get; set; }

        public int? StoreId { get; set; }

        public string? ApplicableCategories { get; set; }

        public string? TermsConditions { get; set; }

        public bool IsFeatured { get; set; } = false;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (ValidUntil <= ValidFrom)
            {
                results.Add(new ValidationResult("Valid Until must be after Valid From date", new[] { nameof(ValidUntil) }));
            }

            if (ValidFrom < DateTime.UtcNow.Date)
            {
                results.Add(new ValidationResult("Valid From cannot be in the past", new[] { nameof(ValidFrom) }));
            }

            var validTypes = new[] { "percentage", "fixed_amount", "cashback", "points" };
            if (!validTypes.Contains(Type.ToLower()))
            {
                results.Add(new ValidationResult("Type must be one of: percentage, fixed_amount, cashback, points", new[] { nameof(Type) }));
            }

            if (Type?.ToLower() == "percentage" && Value > 100)
            {
                results.Add(new ValidationResult("Percentage value cannot exceed 100", new[] { nameof(Value) }));
            }

            return results;
        }
    }

    public class UpdateVoucherDTO : IValidatableObject
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Value { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MinPurchaseAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxDiscountAmount { get; set; }

        [Range(1, int.MaxValue)]
        public int? UsageLimit { get; set; }

        [Range(1, int.MaxValue)]
        public int UsagePerUser { get; set; }

        [Required]
        public DateTime ValidUntil { get; set; }

        public string? ApplicableCategories { get; set; }

        public string? TermsConditions { get; set; }

        public bool IsFeatured { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty; // active, inactive

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (ValidUntil <= DateTime.UtcNow)
            {
                results.Add(new ValidationResult("Valid Until must be in the future", new[] { nameof(ValidUntil) }));
            }

            var validStatuses = new[] { "active", "inactive" };
            if (!validStatuses.Contains(Status.ToLower()))
            {
                results.Add(new ValidationResult("Status must be either 'active' or 'inactive'", new[] { nameof(Status) }));
            }

            return results;
        }
    }

    public class UserVoucherDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public string Status { get; set; } = string.Empty; // available, used, expired
        public DateTime ObtainedAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public int? TransactionId { get; set; }
        public VoucherDTO? Voucher { get; set; }
    }

    public class CollectVoucherRequest
    {
        [Required]
        public int VoucherId { get; set; }

        [Required]
        public int UserId { get; set; }
    }

    public class UseVoucherRequest
    {
        [Required]
        public int UserVoucherId { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal OriginalAmount { get; set; }
    }

    public class VoucherUsageDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public int? TransactionId { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime UsedAt { get; set; }
        public VoucherDTO? Voucher { get; set; }
    }

    public class VoucherSummaryDTO
    {
        public int TotalVouchers { get; set; }
        public int AvailableVouchers { get; set; }
        public int UsedVouchers { get; set; }
        public int ExpiredVouchers { get; set; }
        public decimal TotalSavings { get; set; }
        public List<VoucherDTO> FeaturedVouchers { get; set; } = new List<VoucherDTO>();
        public List<UserVoucherDTO> RecentlyCollected { get; set; } = new List<UserVoucherDTO>();
    }
} 