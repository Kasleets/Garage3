using Garage3.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage3.ViewModels
{
    public class OverviewModel
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

        public MembershipType? MembershipType { get; set; }
        public List<SelectListItem> MembershipTypeOptions { get; set; }
    }
}