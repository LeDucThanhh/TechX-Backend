using Microsoft.EntityFrameworkCore;
using TechX.API.Models;

namespace TechX.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all tables
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }
        public DbSet<LoyaltyPoint> LoyaltyPoints { get; set; }
        public DbSet<CashbackTransaction> CashbackTransactions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<UserVoucher> UserVouchers { get; set; }
        public DbSet<VoucherUsage> VoucherUsages { get; set; }
        public DbSet<SpeechTransaction> SpeechTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            
            // User relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Budgets)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Settings)
                .WithOne(s => s.User)
                .HasForeignKey<Setting>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Transaction relationships
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Store)
                .WithMany(s => s.Transactions)
                .HasForeignKey(t => t.StoreId)
                .OnDelete(DeleteBehavior.SetNull);

            // Budget relationships
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Budgets)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Store relationships
            modelBuilder.Entity<Store>()
                .HasMany(s => s.Items)
                .WithOne(i => i.Store)
                .HasForeignKey(i => i.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Receipt relationships
            modelBuilder.Entity<Receipt>()
                .HasMany(r => r.ReceiptItems)
                .WithOne(ri => ri.Receipt)
                .HasForeignKey(ri => ri.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            // Voucher relationships
            modelBuilder.Entity<UserVoucher>()
                .HasOne(uv => uv.User)
                .WithMany(u => u.UserVouchers)
                .HasForeignKey(uv => uv.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserVoucher>()
                .HasOne(uv => uv.Voucher)
                .WithMany(v => v.UserVouchers)
                .HasForeignKey(uv => uv.VoucherId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.GoogleId)
                .IsUnique()
                .HasFilter("\"GoogleId\" IS NOT NULL");

            modelBuilder.Entity<Voucher>()
                .HasIndex(v => v.Code)
                .IsUnique();

            modelBuilder.Entity<UserVoucher>()
                .HasIndex(uv => new { uv.UserId, uv.VoucherId })
                .IsUnique();

            // Configure computed columns
            modelBuilder.Entity<Budget>()
                .Property(b => b.RemainingAmount)
                .HasComputedColumnSql("amount - spent_amount");

            // Configure check constraints (updated for .NET 8)
            modelBuilder.Entity<Transaction>()
                .ToTable(t => t.HasCheckConstraint("CK_Transaction_Amount", "amount > 0"));

            modelBuilder.Entity<Transaction>()
                .ToTable(t => t.HasCheckConstraint("CK_Transaction_Type", "type IN ('income', 'expense')"));

            modelBuilder.Entity<Category>()
                .ToTable(t => t.HasCheckConstraint("CK_Category_Type", "type IN ('income', 'expense')"));

            modelBuilder.Entity<Budget>()
                .ToTable(t => t.HasCheckConstraint("CK_Budget_Amount", "amount > 0"));

            modelBuilder.Entity<Budget>()
                .ToTable(t => t.HasCheckConstraint("CK_Budget_Period", "period IN ('daily', 'weekly', 'monthly', 'yearly')"));

            modelBuilder.Entity<Store>()
                .ToTable(t => t.HasCheckConstraint("CK_Store_CashbackRate", "cashback_rate >= 0 AND cashback_rate <= 100"));

            modelBuilder.Entity<Store>()
                .ToTable(t => t.HasCheckConstraint("CK_Store_Rating", "rating >= 0 AND rating <= 5"));

            modelBuilder.Entity<Voucher>()
                .ToTable(t => t.HasCheckConstraint("CK_Voucher_Type", "type IN ('percentage', 'fixed_amount', 'cashback', 'points')"));

            modelBuilder.Entity<Voucher>()
                .ToTable(t => t.HasCheckConstraint("CK_Voucher_Value", "value > 0"));

            // Configure default values
            modelBuilder.Entity<User>()
                .Property(u => u.AuthProvider)
                .HasDefaultValue("Email");

            modelBuilder.Entity<User>()
                .Property(u => u.IsEmailVerified)
                .HasDefaultValue(false);

            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Category>()
                .Property(c => c.IsDefault)
                .HasDefaultValue(false);

            modelBuilder.Entity<Category>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Store>()
                .Property(s => s.CashbackRate)
                .HasDefaultValue(0);

            modelBuilder.Entity<Store>()
                .Property(s => s.PointsRate)
                .HasDefaultValue(0);

            modelBuilder.Entity<Store>()
                .Property(s => s.Rating)
                .HasDefaultValue(0);

            modelBuilder.Entity<Store>()
                .Property(s => s.ReviewCount)
                .HasDefaultValue(0);

            modelBuilder.Entity<Store>()
                .Property(s => s.IsPartner)
                .HasDefaultValue(false);

            modelBuilder.Entity<Store>()
                .Property(s => s.IsActive)
                .HasDefaultValue(true);

            // Configure timestamp defaults
            var dateTimeNow = DateTime.UtcNow;
            
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Apply this pattern to all entities with timestamps
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var createdAtProperty = entityType.FindProperty("CreatedAt");
                if (createdAtProperty != null)
                {
                    createdAtProperty.SetDefaultValueSql("CURRENT_TIMESTAMP");
                }

                var updatedAtProperty = entityType.FindProperty("UpdatedAt");
                if (updatedAtProperty != null)
                {
                    updatedAtProperty.SetDefaultValueSql("CURRENT_TIMESTAMP");
                }
            }
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .Where(e => e.Entity.GetType().GetProperty("UpdatedAt") != null);

            foreach (var entry in entries)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
} 