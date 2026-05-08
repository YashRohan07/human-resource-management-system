using HRMS.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Data;

// Main EF Core database context for the HRMS API
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Authentication users are kept separate from employee records
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    // Main HR records
    public DbSet<Employee> Employees => Set<Employee>();

    // Current salary setup for employees
    public DbSet<Salary> Salaries => Set<Salary>();

    // Monthly payroll records
    public DbSet<Payroll> Payrolls => Set<Payroll>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureAppUser(modelBuilder);
        ConfigureEmployee(modelBuilder);
        ConfigureSalary(modelBuilder);
        ConfigurePayroll(modelBuilder);
    }

    private static void ConfigureAppUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("AppUsers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            // Login email should be unique
            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.Property(x => x.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.Role)
                .HasMaxLength(50)
                .IsRequired();
        });
    }

    private static void ConfigureEmployee(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            // Prevent duplicate employee email addresses
            entity.HasIndex(x => x.Email)
                .IsUnique()
                .HasDatabaseName("IX_Employee_Email");

            entity.Property(x => x.Phone)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Department)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Position)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.AccountNumber)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.EmploymentStatus)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            // Soft deleted employees are hidden from normal queries
            entity.HasQueryFilter(x => !x.IsDeleted);
        });
    }

    private static void ConfigureSalary(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Salary>(entity =>
        {
            entity.ToTable("Salaries");

            entity.HasKey(x => x.Id);

            // Salary values need fixed decimal precision
            entity.Property(x => x.BasicSalary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.Bonus)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            entity.Property(x => x.Deduction)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            entity.Property(x => x.EffectiveFrom)
                .IsRequired();

            // Employee can exist without salary, but salary must belong to an employee
            entity.HasOne(x => x.Employee)
                .WithOne(x => x.Salary)
                .HasForeignKey<Salary>(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // One employee can have only one current salary
            entity.HasIndex(x => x.EmployeeId)
                .IsUnique();
        });
    }

    private static void ConfigurePayroll(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.ToTable("Payrolls");

            entity.HasKey(x => x.Id);

            // Snapshot values keep old payroll unchanged after salary updates
            entity.Property(x => x.BasicSalarySnapshot)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.BonusSnapshot)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.DeductionSnapshot)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.Tax)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.GrossSalary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.NetSalary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.GeneratedAt)
                .IsRequired();

            // One employee can have payroll records for many months
            entity.HasOne(x => x.Employee)
                .WithMany(x => x.Payrolls)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate payroll for the same employee and month
            entity.HasIndex(x => new
            {
                x.EmployeeId,
                x.Month,
                x.Year
            })
            .IsUnique()
            .HasDatabaseName("IX_Payroll_Employee_Month_Year");

            // Used when filtering payrolls by month and year
            entity.HasIndex(x => new
            {
                x.Month,
                x.Year
            })
            .HasDatabaseName("IX_Payroll_Month_Year");
        });
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(entry =>
                entry.State == EntityState.Added ||
                entry.State == EntityState.Modified);

        var currentTime = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = currentTime;
                entry.Entity.UpdatedAt = currentTime;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = currentTime;
            }
        }
    }
}