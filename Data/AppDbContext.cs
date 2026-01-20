using Microsoft.EntityFrameworkCore;
using SSO.Client.VAMS.Models;

namespace SSO.Client.VAMS.Data
{
    /// <summary>
    /// Entity Framework Core database context for local application data.
    /// Maps the read-only view <c>vw_PersonnelDivisionDetails</c> to <see cref="PersonnelDivisionDetail"/>.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Constructs a new <see cref="AppDbContext"/>.
        /// </summary>
        /// <param name="options">Options provided by DI (e.g., connection string, provider).</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Represents the view <c>vw_PersonnelDivisionDetails</c> mapped to the <see cref="PersonnelDivisionDetail"/> entity.
        /// This is read-only (backed by a database view).
        /// </summary>
        public DbSet<PersonnelDivisionDetail> PersonnelDivisionDetails { get; set; } = null!;

        /// <summary>
        /// Configures EF model mappings. The personnel details entity is mapped to a DB view.
        /// </summary>
        /// <param name="modelBuilder">Model builder provided by EF Core.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the DTO to the database view. Use property column names that match the view's schema.
            modelBuilder.Entity<PersonnelDivisionDetail>(entity =>
            {
                // Configure a key for EF even though the view is read-only. This helps EF track instances.
                entity.HasKey(e => e.employee_id);
                entity.ToView("vw_PersonnelDivisionDetails");
                entity.Property(e => e.employee_id).HasColumnName("employee_id");
                entity.Property(e => e.surname).HasColumnName("surname");
                entity.Property(e => e.given_name).HasColumnName("given_name");
                entity.Property(e => e.division_name).HasColumnName("division_name");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}