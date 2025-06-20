using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class LoyaltyPointsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? StoreId { get; set; }
        public string? StoreName { get; set; }
        public int Points { get; set; }
        public decimal PointsValue { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UsedAt { get; set; }
    }

    public class CreateLoyaltyPointsDTO : IValidatableObject
    {
        [Required(ErrorMessage = "User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
        public int UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int? StoreId { get; set; }

        [StringLength(255, ErrorMessage = "Store name cannot exceed 255 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s&\-_'.]+$", ErrorMessage = "Store name contains invalid characters")]
        public string? StoreName { get; set; }

        [Required(ErrorMessage = "Points amount is required")]
        [Range(1, 999999, ErrorMessage = "Points must be between 1 and 999,999")]
        public int Points { get; set; }

        [Required(ErrorMessage = "Points value is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Points value must be between 0.01 and 999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Points value can have maximum 2 decimal places")]
        public decimal PointsValue { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        [RegularExpression(@"^(active|used|expired)$", ErrorMessage = "Status must be active, used, or expired")]
        public string Status { get; set; } = "active";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Expiry date validation
            if (ExpiryDate <= DateTime.Now.Date)
            {
                yield return new ValidationResult("Expiry date must be in the future", new[] { nameof(ExpiryDate) });
            }
            
            if (ExpiryDate > DateTime.Now.AddYears(5))
            {
                yield return new ValidationResult("Expiry date cannot be more than 5 years in the future", new[] { nameof(ExpiryDate) });
            }

            // Store validation
            if (!StoreId.HasValue && string.IsNullOrEmpty(StoreName))
            {
                yield return new ValidationResult("Either Store ID or Store Name must be provided", 
                    new[] { nameof(StoreId), nameof(StoreName) });
            }

            // Points value validation - ensure reasonable conversion rate
            var pointsPerDollar = Points / PointsValue;
            if (pointsPerDollar < 0.1m || pointsPerDollar > 1000m)
            {
                yield return new ValidationResult("Points to value ratio appears unrealistic", 
                    new[] { nameof(Points), nameof(PointsValue) });
            }
        }
    }

    public class UpdateLoyaltyPointsDTO : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int? StoreId { get; set; }

        [StringLength(255, ErrorMessage = "Store name cannot exceed 255 characters")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s&\-_'.]+$", ErrorMessage = "Store name contains invalid characters")]
        public string? StoreName { get; set; }

        [Range(1, 999999, ErrorMessage = "Points must be between 1 and 999,999")]
        public int? Points { get; set; }

        [Range(0.01, 999999.99, ErrorMessage = "Points value must be between 0.01 and 999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Points value can have maximum 2 decimal places")]
        public decimal? PointsValue { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        [RegularExpression(@"^(active|used|expired)$", ErrorMessage = "Status must be active, used, or expired")]
        public string? Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UsedAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Expiry date validation
            if (ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.Now.Date)
            {
                yield return new ValidationResult("Expiry date must be in the future", new[] { nameof(ExpiryDate) });
            }
            
            if (ExpiryDate.HasValue && ExpiryDate.Value > DateTime.Now.AddYears(5))
            {
                yield return new ValidationResult("Expiry date cannot be more than 5 years in the future", new[] { nameof(ExpiryDate) });
            }

            // Status-specific validations
            if (Status == "used" && !UsedAt.HasValue)
            {
                yield return new ValidationResult("Used date is required when status is used", new[] { nameof(UsedAt) });
            }

            if (Status == "expired" && ExpiryDate.HasValue && ExpiryDate.Value > DateTime.Now.Date)
            {
                yield return new ValidationResult("Cannot set status to expired if expiry date is in the future", new[] { nameof(Status) });
            }

            // UsedAt validation
            if (UsedAt.HasValue && UsedAt.Value > DateTime.Now)
            {
                yield return new ValidationResult("Used date cannot be in the future", new[] { nameof(UsedAt) });
            }

            // Points value validation
            if (Points.HasValue && PointsValue.HasValue)
            {
                var pointsPerDollar = Points.Value / PointsValue.Value;
                if (pointsPerDollar < 0.1m || pointsPerDollar > 1000m)
                {
                    yield return new ValidationResult("Points to value ratio appears unrealistic", 
                        new[] { nameof(Points), nameof(PointsValue) });
                }
            }
        }
    }

    public class LoyaltySummaryDTO
    {
        public int TotalActivePoints { get; set; }
        public decimal TotalActiveValue { get; set; }
        public int TotalUsedPoints { get; set; }
        public decimal TotalUsedValue { get; set; }
        public int TotalExpiredPoints { get; set; }
        public decimal TotalExpiredValue { get; set; }
        public int PointsExpiringIn30Days { get; set; }
        public decimal ValueExpiringIn30Days { get; set; }
    }
}