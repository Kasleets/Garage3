using Garage3.Controllers;
using Garage3.Data;
using Garage3.Models.Entities;
using Garage3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Garage3.xUnitTests
{
    public class VehicleControllerTests : TestBase
    {
        #region Legacy Overview Test
        //[Fact]
        //public async Task Overview_ReturnsViewResult_WithListOfVehicles()
        //{
        //    // Arrange
        //    using var context = CreateDbContext();
        //    //SeedVehicles(context);
        //    var controller = new VehicleController(context);

        //    // Act
        //    var result = await controller.Overview(null, null);

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    //var model = Assert.IsAssignableFrom<IEnumerable<dynamic>>(viewResult.Model); // Adjusted to 'dynamic'
        //    var model = Assert.IsAssignableFrom<IEnumerable<VehicleOverviewViewModel>>(viewResult.Model); // Adjusted to 'VehicleOverviewViewModel'
        //    Assert.NotNull(model);
        //    Assert.True(model.Any()); // Check that there are vehicles present in the model
        //}

        //[Fact]
        //public async Task Overview_ReturnsViewResult_WithListOfVehicles()
        //{
        //    // Arrange
        //    using var context = CreateDbContext();
        //    var controller = new VehicleController(context);

        //    // Act
        //    var result = await controller.Overview(null, null);


        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsAssignableFrom<IEnumerable<VehicleOverviewViewModel>>(viewResult.Model);
        //    Assert.NotNull(model);
        //    Assert.True(model.Any()); // Check that there are vehicles present in the model



        //}
        #endregion
        [Fact]
        public async Task Overview_ReturnsViewResult_WithCorrectModelType()
        {
            // Arrange
            using var context = CreateDbContext();
            //SeedVehicles(context);
            var controller = new VehicleController(context);

            // Act
            var result = await controller.Overview(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<VehicleOverviewViewModel>>(viewResult.ViewData.Model);
            Assert.NotNull(model);
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
            //SeedVehicles(context);
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
            //SeedVehicles(context);
            var controller = new VehicleController(context)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };

            var vehicleToDelete = context.Vehicles.LastOrDefault(); // Get the last vehicle to delete
            int vehicleToDeleteId = vehicleToDelete.VehicleID;

            // Act
            var result = await controller.DeleteConfirmed(vehicleToDeleteId);

            // Assert
            var deletedVehicle = context.Vehicles.FirstOrDefault(v => v.VehicleID == vehicleToDeleteId);
            Assert.Null(deletedVehicle);
        }
        #region Legacy xUnit for Park
        //[Fact]
        //public async Task Park_ValidVehicle_CreatesNewParkingRecord()
        //{
        //    // Arrange
        //    // Had to make sure to test the POST method, not the GET method

        //    using var context = CreateDbContext();
        //    //SeedVehicles(context);
        //    var controller = new VehicleController(context)
        //    {
        //        TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
        //    };

        //    var vehicleToPark = context.Vehicles.First(); // Get the first vehicle to park
        //    int vehicleToParkId = vehicleToPark.VehicleID;

        //    var parkingRecord = new ParkingRecord
        //    {
        //        VehicleID = vehicleToParkId,
        //        MemberID = 1, // Assuming a valid MemberID, adjust as necessary
        //        ParkTime = DateTime.Now
        //    };

        //    // Act
        //    var result = await controller.Park(vehicleToParkId, parkingRecord);

        //    // Assert
        //    var createdParkingRecord = context.ParkingRecords.FirstOrDefault(pr => pr.VehicleID == vehicleToParkId);
        //    Assert.NotNull(createdParkingRecord);
        //    Assert.Null(createdParkingRecord.CheckOutTime); // Check that the vehicle is currently parked (no checkout time)
        //}
        #endregion
        [Fact]
        public async Task Park_ValidVehicle_CreatesNewParkingRecord()
        {
            // Arrange
            using var context = CreateDbContext();
            var controller = new VehicleController(context)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };

            var vehicleToPark = context.Vehicles.First(); // Get the first vehicle to park

            var parkViewModel = new ParkViewModel
            {
                VehicleId = vehicleToPark.VehicleID,
                MemberId = 1, // Assuming a valid MemberID, adjust as necessary
                ParkTime = DateTime.Now
            };

            // Act
            var result = await controller.Park(parkViewModel); // Pass the ViewModel instance directly

            // Assert
            var createdParkingRecord = context.ParkingRecords.FirstOrDefault(pr => pr.VehicleID == vehicleToPark.VehicleID);
            Assert.NotNull(createdParkingRecord);
            Assert.Equal(vehicleToPark.VehicleID, createdParkingRecord.VehicleID);
            Assert.Equal(1, createdParkingRecord.MemberID); // The MemberID should match what was set
            Assert.Null(createdParkingRecord.CheckOutTime); // Check that the vehicle is currently parked (no checkout time)
        }


        [Fact]
        public async Task UnparkConfirmed_ValidVehicle_UpdatesParkingRecordWithCheckoutTime()
        {
            // Arrange
            using var context = CreateDbContext();
            //SeedVehicles(context);
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
