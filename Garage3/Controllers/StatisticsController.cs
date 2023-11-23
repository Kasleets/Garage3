using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Garage3.Data;
using Garage3.ViewModels;

namespace Garage3.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ParkingDbContext _context;

        public StatisticsController(ParkingDbContext context)
        {
            _context = context;
        }

        public IActionResult Statistics()
        {
            var statisticsViewModel = GetGarageStatistics();
            return View(statisticsViewModel);
        }

        private StatisticsViewModel GetGarageStatistics()
        {
            var totalVehicles = _context.Vehicles.Count();
            var vehiclesByType = _context.Vehicles
                .GroupBy(v => v.VehicleType.TypeName)
                .ToDictionary(g => g.Key, g => g.Count());
            var totalWheels = _context.Vehicles.Sum(v => v.NumberOfWheels);
            var totalRevenue = CalculateTotalRevenue();

            var statisticsViewModel = new StatisticsViewModel
            {
                TotalVehicles = totalVehicles,
                VehiclesByType = vehiclesByType,
                TotalWheels = totalWheels,
                TotalRevenue = totalRevenue
            };

            return statisticsViewModel;
        }

        private decimal CalculateTotalRevenue()
        {
            return 0; // Replace with actual calculation after adding receipt / cost for vehicles
        }

        public IActionResult Vehicles()
        {
            var vehicles = _context.Vehicles.Include(v => v.VehicleType).ToList();
            return View(vehicles);
        }

        public IActionResult Wheels()
        {
            var totalWheels = _context.Vehicles.Sum(v => v.NumberOfWheels);
            return View(totalWheels);
        }

        public IActionResult Revenue()
        {
            var totalRevenue = CalculateTotalRevenue();
            return View(totalRevenue);
        }
    }
}
