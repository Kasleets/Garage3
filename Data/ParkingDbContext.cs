using Garage3.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace Garage3.Data
{
    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<ParkingRecord> ParkingRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Member configurations
            modelBuilder.Entity<Member>()
                .Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Member>()
                .Property(m => m.LastName)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Member>()
                .Property(m => m.PersonalNumber)
                .IsRequired()
                .HasMaxLength(13);
            // Note: Personal Number has 13 because there is a dash after 8 digits.

            // Vehicle configurations
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Brand)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Color)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.NumberOfWheels)
                .IsRequired();

            // Relationships
            modelBuilder.Entity<Member>()
                .HasOne(m => m.Account)
                .WithOne(a => a.Member)
                .HasForeignKey<Account>(a => a.MemberID);

            modelBuilder.Entity<Member>()
                .HasMany(m => m.Vehicles)
                .WithOne(v => v.Owner)
                .HasForeignKey(v => v.OwnerID);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleType)
                .WithMany(t => t.Vehicles)
                .HasForeignKey(v => v.VehicleTypeID);

            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.ParkingRecords)
                .WithOne(p => p.Vehicle)
                .HasForeignKey(p => p.VehicleID)
                .OnDelete(DeleteBehavior.Restrict); // Adjusted to prevent cascade delete issue

            modelBuilder.Entity<Member>()
                .HasMany(m => m.ParkingRecords)
                .WithOne(p => p.Member)
                .HasForeignKey(p => p.MemberID)
                .OnDelete(DeleteBehavior.Restrict); // Adjusted to prevent cascade delete issue

            // Call base method
            base.OnModelCreating(modelBuilder);
        }
    }
}
