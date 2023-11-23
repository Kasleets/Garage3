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

            // VehicleType configurations
            modelBuilder.Entity<VehicleType>().HasData(
                new VehicleType { VehicleTypeID = 1, TypeName = "Car" },
                new VehicleType { VehicleTypeID = 2, TypeName = "Truck" },
                new VehicleType { VehicleTypeID = 3, TypeName = "Motorcycle" },
                new VehicleType { VehicleTypeID = 4, TypeName = "Bus" },
                new VehicleType { VehicleTypeID = 5, TypeName = "Airplane" });

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

            // Adding Seed Data manually typed in
            modelBuilder.Entity<Member>().HasData(
                new Member { MemberID = 1, PersonalNumber = "19501230-1234", FirstName = "Alice", LastName = "Johnson", Age = 73 },
                new Member { MemberID = 2, PersonalNumber = "19800216-2345", FirstName = "Bob", LastName = "Smith", Age = 43 },
                new Member { MemberID = 3, PersonalNumber = "19950721-3456", FirstName = "Carol", LastName = "Davis", Age = 28 },
                new Member { MemberID = 4, PersonalNumber = "19631005-4567", FirstName = "David", LastName = "Martinez", Age = 60 },
                new Member { MemberID = 5, PersonalNumber = "19780819-5678", FirstName = "Eve", LastName = "Garcia", Age = 46 },
                new Member { MemberID = 6, PersonalNumber = "20011212-6789", FirstName = "Frank", LastName = "Lee", Age = 22 }
                );

            // Seed Data for VehicleType
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { VehicleID = 1, OwnerID = 1, RegistrationNumber = "ABC123", Brand = "Toyota", Model = "Corolla", Color = "Blue", NumberOfWheels = 4, VehicleTypeID = 1 },
                new Vehicle { VehicleID = 2, OwnerID = 2, RegistrationNumber = "XYZ789", Brand = "Honda", Model = "Civic", Color = "Red", NumberOfWheels = 4, VehicleTypeID = 1 },
                new Vehicle { VehicleID = 3, OwnerID = 3, RegistrationNumber = "DEF456", Brand = "Ford", Model = "Fiesta", Color = "Green", NumberOfWheels = 4, VehicleTypeID = 1 },
                new Vehicle { VehicleID = 4, OwnerID = 4, RegistrationNumber = "GHI789", Brand = "Volvo", Model = "V70", Color = "Black", NumberOfWheels = 4, VehicleTypeID = 1 },
                new Vehicle { VehicleID = 5, OwnerID = 5, RegistrationNumber = "JKL012", Brand = "Saab", Model = "900", Color = "White", NumberOfWheels = 4, VehicleTypeID = 1 },
                new Vehicle { VehicleID = 6, OwnerID = 6, RegistrationNumber = "MNO345", Brand = "Volkswagen", Model = "Golf", Color = "Silver", NumberOfWheels = 4, VehicleTypeID = 1 }
                );

            modelBuilder.Entity<ParkingRecord>().HasData(
                new ParkingRecord { ParkingRecordID = 1, VehicleID = 1, MemberID = 1, ParkTime = DateTime.Now.AddHours(-3), CheckOutTime = null }, // Vehicle 1 is parked
                new ParkingRecord { ParkingRecordID = 2, VehicleID = 2, MemberID = 2, ParkTime = DateTime.Now.AddHours(-1), CheckOutTime = null },  // Vehicle 2 is parked
                new ParkingRecord { ParkingRecordID = 3, VehicleID = 3, MemberID = 3, ParkTime = DateTime.Now.AddHours(-49), CheckOutTime = null }  // Vehicle 3 is parked
                
                // Add more records as needed
                );

            // Seed Data for Accounts
            modelBuilder.Entity<Account>().HasData(
                new Account { AccountID = 1, MemberID = 1 },
                new Account { AccountID = 2, MemberID = 2 },
                new Account { AccountID = 3, MemberID = 3 },
                new Account { AccountID = 4, MemberID = 4 },
                new Account { AccountID = 5, MemberID = 5 },
                new Account { AccountID = 6, MemberID = 6 }
                );


            // Call base method
            base.OnModelCreating(modelBuilder);
        }
    }
}
