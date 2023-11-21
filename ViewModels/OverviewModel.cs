using Garage3.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage3.ViewModels
{
    public class OverviewModel
    {
#nullable disable
        public int VehicleID { get; set; }
        public string OwnerFullName { get; set; }
       
        public string VehicleType { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? ParkTime { get; set; }

        public MembershipType? MembershipType { get; set; }
        public List<SelectListItem> MembershipTypeOptions { get; set; }
    }
}
