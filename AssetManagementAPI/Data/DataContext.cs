using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<MaintenanceRecord.MaintenanceAction>(name: "maintenance_action");
            modelBuilder.HasPostgresEnum<Transaction.TransactionType>(name: "transaction_type");

            /*modelBuilder.Entity<MaintenanceRecord>()
                .Property(e => e.Action)
                .HasConversion<string>()
                .HasColumnType("maintenance_action");

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Type)
                .HasConversion<string>()
                .HasColumnType("transaction_type");*/

            // Relationships
            modelBuilder.Entity<Department>()
                .HasMany<Employee>()
                .WithOne(e => e.Department)
                .IsRequired(false);

            modelBuilder.Entity<Department>()
                .HasMany<Asset>()
                .WithOne(e => e.Proprietor)
                .IsRequired(false);

            modelBuilder.Entity<Employee>()
                .HasMany<Asset>()
                .WithOne(e => e.Custodian)
                .IsRequired(false);

            modelBuilder.Entity<Employee>()
                .HasMany<MaintenanceRecord>()
                .WithOne(e => e.Documentor)
                .IsRequired(false);

            modelBuilder.Entity<Employee>()
                .HasMany<Transaction>()
                .WithOne(e => e.Transactor)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasMany<Transaction>()
                .WithOne(e => e.Transactee)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasMany<Transaction>()
                .WithOne(e => e.Approver)
                .IsRequired(false);

            // Indexes
            modelBuilder.Entity<Asset>()
                .HasIndex(e => new { e.Type, e.Name })
                .HasMethod("GIN")
                .IsTsVectorExpressionIndex("simple");

            modelBuilder.Entity<Department>()
                .HasIndex(e => new { e.Name })
                .IsUnique()
                .IsTsVectorExpressionIndex("simple");

            modelBuilder.Entity<Employee>()
                .HasIndex(e => new { e.LastName, e.FirstName, e.MiddleName })
                .HasMethod("GIN")
                .IsTsVectorExpressionIndex("simple");
        }
    }
}
