using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GAP.Insurance.Domain
{
    public partial class DBInsuranceContext : DbContext
    {
        public DBInsuranceContext()
        {
        }

        public DBInsuranceContext(DbContextOptions<DBInsuranceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerInsurance> CustomerInsurance { get; set; }
        public virtual DbSet<Insurance> Insurance { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerInsurance>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.InsuranceId, e.StartDate });

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerInsurance)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerInsurance_Customer");

                entity.HasOne(d => d.Insurance)
                    .WithMany(p => p.CustomerInsurance)
                    .HasForeignKey(d => d.InsuranceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerInsurance_Insurance");
            });

            modelBuilder.Entity<Insurance>(entity =>
            {
                entity.Property(e => e.InsuranceId).ValueGeneratedNever();

                entity.Property(e => e.Coverage).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.CoverageType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RiskType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });
        }
    }
}
