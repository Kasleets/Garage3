using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage3.ViewModels
{
    public class ParkViewModel
    {
        public int VehicleId { get; set; }
        public int MemberId { get; set; } // Include if the user needs to be selected
        public DateTime ParkTime { get; set; }

        // Lists for dropdowns in the Razor Page
        public SelectList Vehicles { get; set; }
        public SelectList Members { get; set; } // Include if the user needs to be selected

        public ParkViewModel() => ParkTime = DateTime.Now; // Default to current time
    }

}
