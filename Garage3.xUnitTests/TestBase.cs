using Garage3.Controllers;
using Garage3.Data;
using Garage3.Models;
using Garage3.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Garage3.Models.Entities;
using Bogus;

namespace Garage3.xUnitTests
{
    public class TestBase
    {
        protected ParkingDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ParkingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for the database to ensure that each test gets its own instance
                .Options;

            var dbContext = new ParkingDbContext(options);

            // Seed the database
            SeedVehicles(dbContext);

            return dbContext;
        }
        // Implement the SeedVehicles method 
        protected void SeedVehicles(ParkingDbContext context)
        {
            context.Vehicles.Add(new Vehicle { VehicleID = 1, OwnerID = 1, RegistrationNumber = "ABC123", Brand = "Toyota", Model = "Corolla", Color = "Blue", NumberOfWheels = 4, VehicleTypeID = 1 });
            context.Vehicles.Add(new Vehicle { VehicleID = 2, OwnerID = 2, RegistrationNumber = "XYZ789", Brand = "Honda", Model = "Civic", Color = "Red", NumberOfWheels = 4, VehicleTypeID = 1 });
            context.Vehicles.Add(new Vehicle { VehicleID = 3, OwnerID = 3, RegistrationNumber = "DEF456", Brand = "Ford", Model = "Fiesta", Color = "Green", NumberOfWheels = 4, VehicleTypeID = 1 });
            context.Vehicles.Add(new Vehicle { VehicleID = 4, OwnerID = 4, RegistrationNumber = "GHI789", Brand = "Volvo", Model = "V70", Color = "Black", NumberOfWheels = 4, VehicleTypeID = 1 });
            context.Vehicles.Add(new Vehicle { VehicleID = 5, OwnerID = 5, RegistrationNumber = "JKL012", Brand = "Saab", Model = "900", Color = "White", NumberOfWheels = 4, VehicleTypeID = 1 });

            context.ParkingRecords.Add(new ParkingRecord { VehicleID = 1, MemberID = 1, ParkTime = DateTime.Now.AddHours(-2), CheckOutTime = null });
            context.ParkingRecords.Add(new ParkingRecord { VehicleID = 2, MemberID = 2, ParkTime = DateTime.Now.AddHours(-4), CheckOutTime = null });
            // Add as many vehicles as needed for test

            context.SaveChanges();
        }
    }


}