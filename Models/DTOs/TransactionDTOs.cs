using System.ComponentModel.DataAnnotations;

namespace TechX.API.Models.DTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? StoreId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? StoreName { get; set; }
        public string? ReceiptUrl { get; set; }
        public string? Tags { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurringType { get; set; }
        public decimal? CashbackAmount { get; set; }
        public int? PointsEarned { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateTransactionDTO : IValidatableObject
    {
        public int UserId { get; set; } // Will be set from JWT token
        
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int? StoreId { get; set; }
        
        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 999999999.99, ErrorMessage = "Amount must be between 0.01 and 999,999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount can have maximum 2 decimal places")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "Transaction type is required")]
        [StringLength(20, ErrorMessage = "Transaction type cannot exceed 20 characters")]
        [RegularExpression(@"^(income|expense)$", ErrorMessage = "Transaction type must be 'income' or 'expense'")]
        public string Type { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Transaction date is required")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        
        [Url(ErrorMessage = "Receipt URL must be a valid URL")]
        [StringLength(500, ErrorMessage = "Receipt URL cannot exceed 500 characters")]
        public string? ReceiptUrl { get; set; }
        
        [StringLength(1000, ErrorMessage = "Tags cannot exceed 1000 characters")]
        public string? Tags { get; set; }
        
        public bool IsRecurring { get; set; }
        
        [StringLength(20, ErrorMessage = "Recurring type cannot exceed 20 characters")]
        [RegularExpression(@"^(daily|weekly|monthly|yearly)$", ErrorMessage = "Recurring type must be daily, weekly, monthly, or yearly")]
        public string? RecurringType { get; set; }
        
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Date validation
            if (Date > DateTime.Now.AddDays(1))
            {
                yield return new ValidationResult("Transaction date cannot be more than 1 day in the future", new[] { nameof(Date) });
            }
            
            if (Date < DateTime.Now.AddYears(-10))
            {
                yield return new ValidationResult("Transaction date cannot be more than 10 years in the past", new[] { nameof(Date) });
            }

            // Recurring validation
            if (IsRecurring && string.IsNullOrEmpty(RecurringType))
            {
                yield return new ValidationResult("Recurring type is required when transaction is recurring", new[] { nameof(RecurringType) });
            }
            
            if (!IsRecurring && !string.IsNullOrEmpty(RecurringType))
            {
                yield return new ValidationResult("Recurring type should be empty when transaction is not recurring", new[] { nameof(RecurringType) });
            }
        }
    }

    public class UpdateTransactionDTO : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int? StoreId { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
        public int? CategoryId { get; set; }
        
        [Range(0.01, 999999999.99, ErrorMessage = "Amount must be between 0.01 and 999,999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount can have maximum 2 decimal places")]
        public decimal? Amount { get; set; }
        
        [StringLength(20, ErrorMessage = "Transaction type cannot exceed 20 characters")]
        [RegularExpression(@"^(income|expense)$", ErrorMessage = "Transaction type must be 'income' or 'expense'")]
        public string? Type { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }
        
        [Url(ErrorMessage = "Receipt URL must be a valid URL")]
        [StringLength(500, ErrorMessage = "Receipt URL cannot exceed 500 characters")]
        public string? ReceiptUrl { get; set; }
        
        [StringLength(1000, ErrorMessage = "Tags cannot exceed 1000 characters")]
        public string? Tags { get; set; }
        
        public bool? IsRecurring { get; set; }
        
        [StringLength(20, ErrorMessage = "Recurring type cannot exceed 20 characters")]
        [RegularExpression(@"^(daily|weekly|monthly|yearly)$", ErrorMessage = "Recurring type must be daily, weekly, monthly, or yearly")]
        public string? RecurringType { get; set; }
        
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Date validation
            if (Date.HasValue && Date.Value > DateTime.Now.AddDays(1))
            {
                yield return new ValidationResult("Transaction date cannot be more than 1 day in the future", new[] { nameof(Date) });
            }
            
            if (Date.HasValue && Date.Value < DateTime.Now.AddYears(-10))
            {
                yield return new ValidationResult("Transaction date cannot be more than 10 years in the past", new[] { nameof(Date) });
            }

            // Recurring validation
            if (IsRecurring == true && string.IsNullOrEmpty(RecurringType))
            {
                yield return new ValidationResult("Recurring type is required when transaction is recurring", new[] { nameof(RecurringType) });
            }
        }
    }

    public class TransactionFilterDto : IValidatableObject
    {
        [StringLength(20, ErrorMessage = "Transaction type cannot exceed 20 characters")]
        [RegularExpression(@"^(income|expense)$", ErrorMessage = "Transaction type must be 'income' or 'expense'")]
        public string? Type { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
        public int? CategoryId { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int? StoreId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        
        [Range(0, 999999999.99, ErrorMessage = "Minimum amount must be between 0 and 999,999,999.99")]
        public decimal? MinAmount { get; set; }
        
        [Range(0, 999999999.99, ErrorMessage = "Maximum amount must be between 0 and 999,999,999.99")]
        public decimal? MaxAmount { get; set; }
        
        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
        public string? SearchTerm { get; set; }
        
        [Range(1, 1000, ErrorMessage = "Page must be between 1 and 1000")]
        public int Page { get; set; } = 1;
        
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 20;
        
        [StringLength(50, ErrorMessage = "Sort by field cannot exceed 50 characters")]
        [RegularExpression(@"^(Date|Amount|CategoryName|StoreName|Type)$", ErrorMessage = "Invalid sort field")]
        public string SortBy { get; set; } = "Date";
        
        [StringLength(10, ErrorMessage = "Sort order cannot exceed 10 characters")]
        [RegularExpression(@"^(Asc|Desc)$", ErrorMessage = "Sort order must be 'Asc' or 'Desc'")]
        public string SortOrder { get; set; } = "Desc";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue && StartDate.Value > EndDate.Value)
            {
                yield return new ValidationResult("Start date cannot be greater than end date", new[] { nameof(StartDate), nameof(EndDate) });
            }
            
            if (MinAmount.HasValue && MaxAmount.HasValue && MinAmount.Value > MaxAmount.Value)
            {
                yield return new ValidationResult("Minimum amount cannot be greater than maximum amount", new[] { nameof(MinAmount), nameof(MaxAmount) });
            }
        }
    }

    public class CategorySpending
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class TransactionSummaryDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalSaved { get; set; }
        public List<CategorySpending> SpendingByCategory { get; set; } = new List<CategorySpending>();
    }
} 
        }
    }

    public class UpdateTransactionDTO : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int? StoreId { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
        public int? CategoryId { get; set; }
        
        [Range(0.01, 999999999.99, ErrorMessage = "Amount must be between 0.01 and 999,999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount can have maximum 2 decimal places")]
        public decimal? Amount { get; set; }
        
        [StringLength(20, ErrorMessage = "Transaction type cannot exceed 20 characters")]
        [RegularExpression(@"^(income|expense)$", ErrorMessage = "Transaction type must be 'income' or 'expense'")]
        public string? Type { get; set; }
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }
        
        [Url(ErrorMessage = "Receipt URL must be a valid URL")]
        [StringLength(500, ErrorMessage = "Receipt URL cannot exceed 500 characters")]
        public string? ReceiptUrl { get; set; }
        
        [StringLength(1000, ErrorMessage = "Tags cannot exceed 1000 characters")]
        public string? Tags { get; set; }
        
        public bool? IsRecurring { get; set; }
        
        [StringLength(20, ErrorMessage = "Recurring type cannot exceed 20 characters")]
        [RegularExpression(@"^(daily|weekly|monthly|yearly)$", ErrorMessage = "Recurring type must be daily, weekly, monthly, or yearly")]
        public string? RecurringType { get; set; }
        
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Date validation
            if (Date.HasValue && Date.Value > DateTime.Now.AddDays(1))
            {
                yield return new ValidationResult("Transaction date cannot be more than 1 day in the future", new[] { nameof(Date) });
            }
            
            if (Date.HasValue && Date.Value < DateTime.Now.AddYears(-10))
            {
                yield return new ValidationResult("Transaction date cannot be more than 10 years in the past", new[] { nameof(Date) });
            }

            // Recurring validation
            if (IsRecurring == true && string.IsNullOrEmpty(RecurringType))
            {
                yield return new ValidationResult("Recurring type is required when transaction is recurring", new[] { nameof(RecurringType) });
            }
        }
    }

    public class TransactionFilterDto : IValidatableObject
    {
        [StringLength(20, ErrorMessage = "Transaction type cannot exceed 20 characters")]
        [RegularExpression(@"^(income|expense)$", ErrorMessage = "Transaction type must be 'income' or 'expense'")]
        public string? Type { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
        public int? CategoryId { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Store ID must be greater than 0")]
        public int? StoreId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        
        [Range(0, 999999999.99, ErrorMessage = "Minimum amount must be between 0 and 999,999,999.99")]
        public decimal? MinAmount { get; set; }
        
        [Range(0, 999999999.99, ErrorMessage = "Maximum amount must be between 0 and 999,999,999.99")]
        public decimal? MaxAmount { get; set; }
        
        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
        public string? SearchTerm { get; set; }
        
        [Range(1, 1000, ErrorMessage = "Page must be between 1 and 1000")]
        public int Page { get; set; } = 1;
        
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 20;
        
        [StringLength(50, ErrorMessage = "Sort by field cannot exceed 50 characters")]
        [RegularExpression(@"^(Date|Amount|CategoryName|StoreName|Type)$", ErrorMessage = "Invalid sort field")]
        public string SortBy { get; set; } = "Date";
        
        [StringLength(10, ErrorMessage = "Sort order cannot exceed 10 characters")]
        [RegularExpression(@"^(Asc|Desc)$", ErrorMessage = "Sort order must be 'Asc' or 'Desc'")]
        public string SortOrder { get; set; } = "Desc";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue && StartDate.Value > EndDate.Value)
            {
                yield return new ValidationResult("Start date cannot be greater than end date", new[] { nameof(StartDate), nameof(EndDate) });
            }
            
            if (MinAmount.HasValue && MaxAmount.HasValue && MinAmount.Value > MaxAmount.Value)
            {
                yield return new ValidationResult("Minimum amount cannot be greater than maximum amount", new[] { nameof(MinAmount), nameof(MaxAmount) });
            }
        }
    }

    public class CategorySpending
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class TransactionSummaryDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalSaved { get; set; }
        public List<CategorySpending> SpendingByCategory { get; set; } = new List<CategorySpending>();
    }
} 