using Microsoft.AspNetCore.Mvc.Rendering;
using Garage3.Models.Entities;


namespace Garage3.ViewModels
{
    public class VehicleOverviewModel
    {
#nullable disable
        public int VehicleID { get; set; }
        public string OwnerFullName { get; set; }

        //New properties for sorting
        public string SortOrder { get; set; }// property to hold selected sorting option

        public List<SelectListItem> SortOrderItems { get; set; }// List of sorting options as SelectListItem

        public string SearchString { get; set; }
        public string VehicleType { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? ParkTime { get; set; }
        public List<SelectListItem> MembershipTypeOptions { get; set; }

        // Formatted time for the main overview
        public string GetFormattedParkingTime(ParkingRecord vehicle)
        {
            var parkingTime = DateTime.Now - vehicle.ParkTime;
            if (parkingTime.Days > 0)
            {
                return $"est. {parkingTime.Days:D1} days";
            }
            else
            {
                return $"est. {parkingTime.Hours:D1} hours";
            }
        }
    }
}
