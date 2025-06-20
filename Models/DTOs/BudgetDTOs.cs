using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class BudgetDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal Amount { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Period { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateBudgetDTO : IValidatableObject
    {
        [Required(ErrorMessage = "User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "Budget amount is required")]
        [Range(1.00, 999999999.99, ErrorMessage = "Budget amount must be between 1.00 and 999,999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Budget amount can have maximum 2 decimal places")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "Budget period is required")]
        [StringLength(20, ErrorMessage = "Period cannot exceed 20 characters")]
        [RegularExpression(@"^(daily|weekly|monthly|quarterly|yearly)$", ErrorMessage = "Period must be daily, weekly, monthly, quarterly, or yearly")]
        public string Period { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Date validation
            if (StartDate >= EndDate)
            {
                yield return new ValidationResult("End date must be after start date", new[] { nameof(EndDate) });
            }
            
            if (StartDate < DateTime.Now.Date.AddYears(-1))
            {
                yield return new ValidationResult("Start date cannot be more than 1 year in the past", new[] { nameof(StartDate) });
            }
            
            if (EndDate > DateTime.Now.Date.AddYears(5))
            {
                yield return new ValidationResult("End date cannot be more than 5 years in the future", new[] { nameof(EndDate) });
            }

            // Period-specific validation
            var daysDifference = (EndDate - StartDate).Days;
            switch (Period?.ToLower())
            {
                case "daily":
                    if (daysDifference != 1)
                        yield return new ValidationResult("Daily budget should have exactly 1 day duration", new[] { nameof(Period) });
                    break;
                case "weekly":
                    if (daysDifference < 7 || daysDifference > 7)
                        yield return new ValidationResult("Weekly budget should have exactly 7 days duration", new[] { nameof(Period) });
                    break;
                case "monthly":
                    if (daysDifference < 28 || daysDifference > 31)
                        yield return new ValidationResult("Monthly budget should have 28-31 days duration", new[] { nameof(Period) });
                    break;
                case "quarterly":
                    if (daysDifference < 90 || daysDifference > 92)
                        yield return new ValidationResult("Quarterly budget should have approximately 90-92 days duration", new[] { nameof(Period) });
                    break;
                case "yearly":
                    if (daysDifference < 365 || daysDifference > 366)
                        yield return new ValidationResult("Yearly budget should have 365-366 days duration", new[] { nameof(Period) });
                    break;
            }
        }
    }

    public class UpdateBudgetDTO : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
        public int? CategoryId { get; set; }
        
        [Range(1.00, 999999999.99, ErrorMessage = "Budget amount must be between 1.00 and 999,999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Budget amount can have maximum 2 decimal places")]
        public decimal? Amount { get; set; }
        
        [StringLength(20, ErrorMessage = "Period cannot exceed 20 characters")]
        [RegularExpression(@"^(daily|weekly|monthly|quarterly|yearly)$", ErrorMessage = "Period must be daily, weekly, monthly, quarterly, or yearly")]
        public string? Period { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        
        public bool? IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Date validation
            if (StartDate.HasValue && EndDate.HasValue && StartDate.Value >= EndDate.Value)
            {
                yield return new ValidationResult("End date must be after start date", new[] { nameof(EndDate) });
            }
            
            if (StartDate.HasValue && StartDate.Value < DateTime.Now.Date.AddYears(-1))
            {
                yield return new ValidationResult("Start date cannot be more than 1 year in the past", new[] { nameof(StartDate) });
            }
            
            if (EndDate.HasValue && EndDate.Value > DateTime.Now.Date.AddYears(5))
            {
                yield return new ValidationResult("End date cannot be more than 5 years in the future", new[] { nameof(EndDate) });
            }
        }
    }
}