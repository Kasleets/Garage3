using Garage3.Models.Entities;

namespace Garage3.ViewModels
{
    public class VehicleDetailedViewModel
    {
#nullable disable
        public int VehicleID { get; set; }
        public Vehicle Vehicle { get; set; }
        public VehicleType VehicleType { get; set; }

        public ParkingRecord ParkingRecord { get; set; }

        // Navigation property
        public virtual ICollection<Vehicle> Vehicles { get; set; }


        public string GetFormattedParkingTime(ParkingRecord vehicle)
        {
            var parkingTime = DateTime.Now - vehicle.ParkTime;
            string formattedTime = string.Empty;
            if (parkingTime.Days > 0)
            {
                return formattedTime += $"{parkingTime.Days:D1} days: {parkingTime.Hours:D1} hours : {parkingTime.Minutes:D2} minutes : {parkingTime.Seconds:D2} seconds";

            }
            else
            {
                formattedTime += $"{parkingTime.Hours:D1} hours : {parkingTime.Minutes:D2} minutes : {parkingTime.Seconds:D2} seconds";
                return formattedTime;
            }
        }
    }
}

