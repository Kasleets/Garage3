using Garage3.Controllers;
using Garage3.Data;
using Garage3.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Garage3.xUnitTests
{
    public class VehicleControllerTests : TestBase
    {
        // You can add a seeding method here if needed

        [Fact]
        public async Task Overview_ReturnsViewResult_WithListOfVehicles()
        {
            // Arrange
            using var context = CreateDbContext();
            SeedVehicles(context);
            var controller = new VehicleController(context);

            // Act
            var result = await controller.Overview(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<dynamic>>(viewResult.Model); // Adjusted to 'dynamic'
            Assert.NotNull(model);
            Assert.True(model.Any()); // Check that there are vehicles present in the model
        }


        // Implement the SeedVehicles method 
        private void SeedVehicles(ParkingDbContext context)
        {
            // Example:
            context.Vehicles.Add(new Vehicle { VehicleID = 1, OwnerID = 1, RegistrationNumber = "ABC123", Brand = "Toyota", Model = "Corolla", Color = "Blue", NumberOfWheels = 4, VehicleTypeID = 1 });
            context.Vehicles.Add(new Vehicle { VehicleID = 2, OwnerID = 2, RegistrationNumber = "XYZ789", Brand = "Honda", Model = "Civic", Color = "Red", NumberOfWheels = 4, VehicleTypeID = 1 });
            context.Vehicles.Add(new Vehicle { VehicleID = 3, OwnerID = 3, RegistrationNumber = "DEF456", Brand = "Ford", Model = "Fiesta", Color = "Green", NumberOfWheels = 4, VehicleTypeID = 1 });
            context.Vehicles.Add(new Vehicle { VehicleID = 4, OwnerID = 4, RegistrationNumber = "GHI789", Brand = "Volvo", Model = "V70", Color = "Black", NumberOfWheels = 4, VehicleTypeID = 1 });
            context.Vehicles.Add(new Vehicle { VehicleID = 5, OwnerID = 5, RegistrationNumber = "JKL012", Brand = "Saab", Model = "900", Color = "White", NumberOfWheels = 4, VehicleTypeID = 1 });
            
            // Add as many vehicles as needed for your test

            context.SaveChanges();
        }

        [Fact]
        public async Task Add_PostValidVehicle_AddsVehicleToDatabase()
        {
            // Arrange
            using var context = CreateDbContext();

            var controller = new VehicleController(context)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };

            var newVehicle = new Vehicle
            {
                OwnerID = 1, 
                RegistrationNumber = "NEW123",
                Brand = "TestBrand",
                Model = "TestModel",
                Color = "TestColor",
                NumberOfWheels = 4,
                VehicleTypeID = 1 
            };

            // Act
            var result = await controller.Add(newVehicle);

            // Assert
            var addedVehicle = context.Vehicles.FirstOrDefault(v => v.RegistrationNumber == newVehicle.RegistrationNumber);
            Assert.NotNull(addedVehicle);
            Assert.Equal("TestBrand", addedVehicle.Brand);
        }

        [Fact]
        public async Task Edit_PostValidVehicle_UpdatesVehicleInDatabase()
        {
            // Arrange
            using var context = CreateDbContext();
            SeedVehicles(context);
            var controller = new VehicleController(context)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };

            var vehicleToEdit = context.Vehicles.First(); // Get the first vehicle to edit
            vehicleToEdit.Brand = "UpdatedBrand"; // Change some details

            // Act
            var result = await controller.Edit(vehicleToEdit.VehicleID, vehicleToEdit);

            // Assert
            var updatedVehicle = context.Vehicles.FirstOrDefault(v => v.VehicleID == vehicleToEdit.VehicleID);
            Assert.NotNull(updatedVehicle);
            Assert.Equal("UpdatedBrand", updatedVehicle.Brand);
            // ... other assertions
        }

        [Fact]
        public async Task DeleteConfirmed_ValidVehicle_RemovesVehicleFromDatabase()
        {
            // Arrange
            using var context = CreateDbContext();
            SeedVehicles(context);
            var controller = new VehicleController(context)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };

            var vehicleToDelete = context.Vehicles.First(); // Get the first vehicle to delete
            int vehicleToDeleteId = vehicleToDelete.VehicleID;

            // Act
            var result = await controller.DeleteConfirmed(vehicleToDeleteId);

            // Assert
            var deletedVehicle = context.Vehicles.FirstOrDefault(v => v.VehicleID == vehicleToDeleteId);
            Assert.Null(deletedVehicle);
        }

        [Fact]
        public async Task Park_ValidVehicle_CreatesNewParkingRecord()
        {
            // Arrange
            // Had to make sure to test the POST method, not the GET method

            using var context = CreateDbContext();
            SeedVehicles(context);
            var controller = new VehicleController(context)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };

            var vehicleToPark = context.Vehicles.First(); // Get the first vehicle to park
            int vehicleToParkId = vehicleToPark.VehicleID;

            var parkingRecord = new ParkingRecord
            {
                VehicleID = vehicleToParkId,
                MemberID = 1, // Assuming a valid MemberID, adjust as necessary
                ParkTime = DateTime.Now
            };

            // Act
            var result = await controller.Park(vehicleToParkId, parkingRecord);

            // Assert
            var createdParkingRecord = context.ParkingRecords.FirstOrDefault(pr => pr.VehicleID == vehicleToParkId);
            Assert.NotNull(createdParkingRecord);
            Assert.Null(createdParkingRecord.CheckOutTime); // Check that the vehicle is currently parked (no checkout time)
        }

        [Fact]
        public async Task UnparkConfirmed_ValidVehicle_UpdatesParkingRecordWithCheckoutTime()
        {
            // Arrange
            using var context = CreateDbContext();
            SeedVehicles(context);
            var controller = new VehicleController(context)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };

            // First, park a vehicle
            var vehicleToUnpark = context.Vehicles.First();
            var parkingRecord = new ParkingRecord { VehicleID = vehicleToUnpark.VehicleID, ParkTime = DateTime.Now };
            context.ParkingRecords.Add(parkingRecord);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.UnparkConfirmed(vehicleToUnpark.VehicleID);

            // Assert
            var updatedParkingRecord = context.ParkingRecords.FirstOrDefault(pr => pr.VehicleID == vehicleToUnpark.VehicleID);
            Assert.NotNull(updatedParkingRecord);
            Assert.NotNull(updatedParkingRecord.CheckOutTime); // Check that the checkout time is set
        }


    }
}
