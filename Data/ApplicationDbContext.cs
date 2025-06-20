using Microsoft.EntityFrameworkCore;
using TechX.API.Models;

namespace TechX.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }
        public DbSet<LoyaltyPoints> LoyaltyPoints { get; set; }
        public DbSet<CashbackTransaction> CashbackTransactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<SpeechTransaction> SpeechTransactions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Avatar).HasMaxLength(500);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Preferences).HasColumnType("jsonb");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                // Google Authentication fields
                entity.Property(e => e.GoogleId).HasMaxLength(255);
                entity.Property(e => e.GooglePicture).HasMaxLength(500);
                entity.Property(e => e.AuthProvider).HasMaxLength(10).HasDefaultValue("Email");
                
                // OTP fields
                entity.Property(e => e.OtpCode).HasMaxLength(6);
                entity.Property(e => e.OtpAttempts).HasDefaultValue(0);
                
                // Index for Google authentication
                entity.HasIndex(e => e.GoogleId).IsUnique().HasFilter("\"GoogleId\" IS NOT NULL");
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Icon).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.IsDefault).HasDefaultValue(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Type).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CategoryId).IsRequired();
                entity.Property(e => e.CategoryName).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.StoreName).HasMaxLength(255);
                entity.Property(e => e.ReceiptUrl).HasMaxLength(500);
                entity.Property(e => e.Tags).HasColumnType("jsonb");
                entity.Property(e => e.RecurringType).HasMaxLength(20);
                entity.Property(e => e.CashbackAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Transactions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Transactions)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.Store)
                    .WithMany()
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Store configuration
            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Logo).HasMaxLength(500);
                entity.Property(e => e.Banner).HasMaxLength(500);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Latitude).HasColumnType("decimal(10,8)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(11,8)");
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Website).HasMaxLength(500);
                entity.Property(e => e.OperatingHours).HasColumnType("jsonb");
                entity.Property(e => e.CashbackRate).HasColumnType("decimal(5,2)").HasDefaultValue(0);
                entity.Property(e => e.PointsRate).HasColumnType("decimal(5,2)").HasDefaultValue(0);
                entity.Property(e => e.Rating).HasColumnType("decimal(3,2)").HasDefaultValue(0);
                entity.Property(e => e.ReviewCount).HasDefaultValue(0);
                entity.Property(e => e.Distance).HasColumnType("decimal(10,2)");
                entity.Property(e => e.IsPartner).HasDefaultValue(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Item configuration
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StoreId).IsRequired();
                entity.Property(e => e.StoreName).HasMaxLength(255);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Images).HasColumnType("jsonb");
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.Tags).HasColumnType("jsonb");
                entity.Property(e => e.IsAvailable).HasDefaultValue(true);
                entity.Property(e => e.StockQuantity);
                entity.Property(e => e.Rating).HasColumnType("decimal(3,2)");
                entity.Property(e => e.ReviewCount);
                entity.Property(e => e.IsFeatured).HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.Store)
                    .WithMany(e => e.Items)
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Receipt configuration
            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.StoreId);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.ImageUrl).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("pending");
                entity.Property(e => e.CashbackAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PointsEarned);
                entity.Property(e => e.OcrText);
                entity.Property(e => e.ProcessedAt);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Receipts)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Store)
                    .WithMany() // A store can have many receipts, but we don't need a navigation property on Store
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ReceiptItem configuration
            modelBuilder.Entity<ReceiptItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ReceiptId).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Quantity).HasDefaultValue(1);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                
                entity.HasOne(e => e.Receipt)
                    .WithMany()
                    .HasForeignKey(e => e.ReceiptId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Budget configuration
            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CategoryId).IsRequired();
                entity.Property(e => e.CategoryName).HasMaxLength(100);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Period).IsRequired().HasMaxLength(20);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                entity.Property(e => e.SpentAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.RemainingAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Alerts).HasColumnType("jsonb");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt);
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Budgets)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Budgets)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // LoyaltyPoints configuration
            modelBuilder.Entity<LoyaltyPoints>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.StoreName).HasMaxLength(255);
                entity.Property(e => e.Points).IsRequired();
                entity.Property(e => e.PointsValue).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.ExpiryDate).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("active");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UsedAt);
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.LoyaltyPointsHistory)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Store)
                    .WithMany(e => e.LoyaltyPoints)
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // CashbackTransaction configuration
            modelBuilder.Entity<CashbackTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.TransactionId);
                entity.Property(e => e.StoreName).HasMaxLength(255);
                entity.Property(e => e.TransactionAmount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.CashbackAmount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.CashbackRate).HasColumnType("decimal(5,2)").IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("pending");
                entity.Property(e => e.TransactionDate).IsRequired();
                entity.Property(e => e.ApprovedDate);
                entity.Property(e => e.PaidDate);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.RejectionReason).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.CashbackTransactions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Store)
                    .WithMany(e => e.CashbackTransactions)
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.SetNull);
                    
                entity.HasOne(e => e.Transaction)
                    .WithMany()
                    .HasForeignKey(e => e.TransactionId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Review configuration
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.StoreId).IsRequired();
                entity.Property(e => e.Rating).IsRequired();
                entity.Property(e => e.Title).HasMaxLength(200);
                entity.Property(e => e.Comment).HasMaxLength(1000);
                entity.Property(e => e.Images).HasColumnType("jsonb");
                entity.Property(e => e.IsVerifiedPurchase).HasDefaultValue(false);
                entity.Property(e => e.HelpfulCount).HasDefaultValue(0);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt);
                
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Store)
                    .WithMany()
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Notification configuration
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Data).HasColumnType("jsonb");
                entity.Property(e => e.IsRead).HasDefaultValue(false);
                entity.Property(e => e.ReadAt);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Notifications)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Settings configuration
            modelBuilder.Entity<Settings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Language).HasMaxLength(10).HasDefaultValue("vi");
                entity.Property(e => e.Currency).HasMaxLength(3).HasDefaultValue("VND");
                entity.Property(e => e.Theme).HasMaxLength(20).HasDefaultValue("light");
                entity.Property(e => e.EnablePushNotifications).HasDefaultValue(true);
                entity.Property(e => e.EnableEmailNotifications).HasDefaultValue(true);
                entity.Property(e => e.BudgetAlerts).HasDefaultValue(true);
                entity.Property(e => e.CashbackAlerts).HasDefaultValue(true);
                entity.Property(e => e.LoyaltyAlerts).HasDefaultValue(true);
                entity.Property(e => e.PrivacySettings).HasColumnType("jsonb");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt);
                
                entity.HasOne(e => e.User)
                    .WithOne(e => e.Settings)
                    .HasForeignKey<Settings>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // SpeechTransaction configuration
            modelBuilder.Entity<SpeechTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.VoiceText).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Language).IsRequired().HasMaxLength(10).HasDefaultValue("vi");
                entity.Property(e => e.ExtractedAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ExtractedCategory).HasMaxLength(100);
                entity.Property(e => e.ExtractedDescription).HasMaxLength(500);
                entity.Property(e => e.ConfidenceScore);
                entity.Property(e => e.ProcessingTimeMs);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("pending");
                entity.Property(e => e.ErrorMessage).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.ProcessedAt);
                
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RefreshToken configuration
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Token).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ExpiryDate).IsRequired();
                entity.Property(e => e.IsRevoked).HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
} 