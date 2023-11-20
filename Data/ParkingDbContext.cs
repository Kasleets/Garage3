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

        // Note: Here you can input any other DbSets for other entities you may have.

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
            // Note: It is 13 long because after 8 digits there is a dash and then 4 more digits. Need validation in a method.

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
                .HasAnnotation("Range", new Range(0, 100));

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
                .HasForeignKey(p => p.VehicleID);

            modelBuilder.Entity<Member>()
                .HasMany(m => m.ParkingRecords)
                .WithOne(p => p.Member)
                .HasForeignKey(p => p.MemberID);

            // Call base method
            base.OnModelCreating(modelBuilder);
        }
    }
}
