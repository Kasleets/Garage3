﻿// <auto-generated />
using System;
using Garage3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Garage3.Migrations
{
    [DbContext(typeof(ParkingDbContext))]
    partial class ParkingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Garage3.Models.Entities.Account", b =>
                {
                    b.Property<int>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountID"));

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.HasKey("AccountID");

                    b.HasIndex("MemberID")
                        .IsUnique();

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            AccountID = 1,
                            MemberID = 1
                        },
                        new
                        {
                            AccountID = 2,
                            MemberID = 2
                        },
                        new
                        {
                            AccountID = 3,
                            MemberID = 3
                        },
                        new
                        {
                            AccountID = 4,
                            MemberID = 4
                        },
                        new
                        {
                            AccountID = 5,
                            MemberID = 5
                        },
                        new
                        {
                            AccountID = 6,
                            MemberID = 6
                        });
                });

            modelBuilder.Entity("Garage3.Models.Entities.Member", b =>
                {
                    b.Property<int>("MemberID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MemberID"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PersonalNumber")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("MemberID");

                    b.ToTable("Members");

                    b.HasData(
                        new
                        {
                            MemberID = 1,
                            Age = 73,
                            FirstName = "Alice",
                            LastName = "Johnson",
                            PersonalNumber = "19501230-1234"
                        },
                        new
                        {
                            MemberID = 2,
                            Age = 43,
                            FirstName = "Bob",
                            LastName = "Smith",
                            PersonalNumber = "19800216-2345"
                        },
                        new
                        {
                            MemberID = 3,
                            Age = 28,
                            FirstName = "Carol",
                            LastName = "Davis",
                            PersonalNumber = "19950721-3456"
                        },
                        new
                        {
                            MemberID = 4,
                            Age = 60,
                            FirstName = "David",
                            LastName = "Martinez",
                            PersonalNumber = "19631005-4567"
                        },
                        new
                        {
                            MemberID = 5,
                            Age = 46,
                            FirstName = "Eve",
                            LastName = "Garcia",
                            PersonalNumber = "19780819-5678"
                        },
                        new
                        {
                            MemberID = 6,
                            Age = 22,
                            FirstName = "Frank",
                            LastName = "Lee",
                            PersonalNumber = "20011212-6789"
                        });
                });

            modelBuilder.Entity("Garage3.Models.Entities.ParkingRecord", b =>
                {
                    b.Property<int>("ParkingRecordID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParkingRecordID"));

                    b.Property<DateTime?>("CheckOutTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("MemberID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ParkTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("VehicleID")
                        .HasColumnType("int");

                    b.HasKey("ParkingRecordID");

                    b.HasIndex("MemberID");

                    b.HasIndex("VehicleID");

                    b.ToTable("ParkingRecords");

                    b.HasData(
                        new
                        {
                            ParkingRecordID = 1,
                            MemberID = 1,
                            ParkTime = new DateTime(2023, 11, 22, 19, 23, 19, 698, DateTimeKind.Local).AddTicks(1178),
                            VehicleID = 1
                        },
                        new
                        {
                            ParkingRecordID = 2,
                            MemberID = 2,
                            ParkTime = new DateTime(2023, 11, 22, 21, 23, 19, 698, DateTimeKind.Local).AddTicks(1183),
                            VehicleID = 2
                        },
                        new
                        {
                            ParkingRecordID = 3,
                            MemberID = 3,
                            ParkTime = new DateTime(2023, 11, 20, 21, 23, 19, 698, DateTimeKind.Local).AddTicks(1186),
                            VehicleID = 3
                        });
                });

            modelBuilder.Entity("Garage3.Models.Entities.Vehicle", b =>
                {
                    b.Property<int>("VehicleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehicleID"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("NumberOfWheels")
                        .HasColumnType("int");

                    b.Property<int>("OwnerID")
                        .HasColumnType("int");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("VehicleTypeID")
                        .HasColumnType("int");

                    b.HasKey("VehicleID");

                    b.HasIndex("OwnerID");

                    b.HasIndex("VehicleTypeID");

                    b.ToTable("Vehicles");

                    b.HasData(
                        new
                        {
                            VehicleID = 1,
                            Brand = "Toyota",
                            Color = "Blue",
                            Model = "Corolla",
                            NumberOfWheels = 4,
                            OwnerID = 1,
                            RegistrationNumber = "ABC123",
                            VehicleTypeID = 1
                        },
                        new
                        {
                            VehicleID = 2,
                            Brand = "Honda",
                            Color = "Red",
                            Model = "Civic",
                            NumberOfWheels = 4,
                            OwnerID = 2,
                            RegistrationNumber = "XYZ789",
                            VehicleTypeID = 1
                        },
                        new
                        {
                            VehicleID = 3,
                            Brand = "Ford",
                            Color = "Green",
                            Model = "Fiesta",
                            NumberOfWheels = 4,
                            OwnerID = 3,
                            RegistrationNumber = "DEF456",
                            VehicleTypeID = 1
                        },
                        new
                        {
                            VehicleID = 4,
                            Brand = "Volvo",
                            Color = "Black",
                            Model = "V70",
                            NumberOfWheels = 4,
                            OwnerID = 4,
                            RegistrationNumber = "GHI789",
                            VehicleTypeID = 1
                        },
                        new
                        {
                            VehicleID = 5,
                            Brand = "Saab",
                            Color = "White",
                            Model = "900",
                            NumberOfWheels = 4,
                            OwnerID = 5,
                            RegistrationNumber = "JKL012",
                            VehicleTypeID = 1
                        },
                        new
                        {
                            VehicleID = 6,
                            Brand = "Volkswagen",
                            Color = "Silver",
                            Model = "Golf",
                            NumberOfWheels = 4,
                            OwnerID = 6,
                            RegistrationNumber = "MNO345",
                            VehicleTypeID = 1
                        });
                });

            modelBuilder.Entity("Garage3.Models.Entities.VehicleType", b =>
                {
                    b.Property<int>("VehicleTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehicleTypeID"));

                    b.Property<string>("TypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VehicleTypeID");

                    b.ToTable("VehicleTypes");

                    b.HasData(
                        new
                        {
                            VehicleTypeID = 1,
                            TypeName = "Car"
                        },
                        new
                        {
                            VehicleTypeID = 2,
                            TypeName = "Truck"
                        },
                        new
                        {
                            VehicleTypeID = 3,
                            TypeName = "Motorcycle"
                        },
                        new
                        {
                            VehicleTypeID = 4,
                            TypeName = "Bus"
                        },
                        new
                        {
                            VehicleTypeID = 5,
                            TypeName = "Airplane"
                        });
                });

            modelBuilder.Entity("Garage3.Models.Entities.Account", b =>
                {
                    b.HasOne("Garage3.Models.Entities.Member", "Member")
                        .WithOne("Account")
                        .HasForeignKey("Garage3.Models.Entities.Account", "MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });

            modelBuilder.Entity("Garage3.Models.Entities.ParkingRecord", b =>
                {
                    b.HasOne("Garage3.Models.Entities.Member", "Member")
                        .WithMany("ParkingRecords")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Garage3.Models.Entities.Vehicle", "Vehicle")
                        .WithMany("ParkingRecords")
                        .HasForeignKey("VehicleID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Garage3.Models.Entities.Vehicle", b =>
                {
                    b.HasOne("Garage3.Models.Entities.Member", "Owner")
                        .WithMany("Vehicles")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Garage3.Models.Entities.VehicleType", "VehicleType")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("Garage3.Models.Entities.Member", b =>
                {
                    b.Navigation("Account");

                    b.Navigation("ParkingRecords");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Garage3.Models.Entities.Vehicle", b =>
                {
                    b.Navigation("ParkingRecords");
                });

            modelBuilder.Entity("Garage3.Models.Entities.VehicleType", b =>
                {
                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
