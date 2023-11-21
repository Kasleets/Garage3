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

            // Seed the database if needed

            return dbContext;
        }
    }


}