namespace Garage3.ViewModels
{
    public class StatisticsViewModel
    {
        public int TotalVehicles { get; set; }
        public Dictionary<string, int> VehiclesByType { get; set; }
        public int TotalWheels { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}

