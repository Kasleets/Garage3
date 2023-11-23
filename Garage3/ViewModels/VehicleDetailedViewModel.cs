using Garage3.Models.Entities;
using System;

namespace Garage3.ViewModels
{
#nullable disable
    public class VehicleDetailedViewModel
    {
        // Vehicle-specific properties
        public int VehicleID { get; set; }
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }

        // Member = Owner-specific properties
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }

        // VehicleType-specific properties
        public string VehicleTypeName { get; set; }

        // ParkingRecord properties - assuming you want to display the most recent or relevant parking record
        public DateTime ParkTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string FormattedParkingDuration { get; set; }

        public VehicleDetailedViewModel(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                VehicleID = vehicle.VehicleID;
                RegistrationNumber = vehicle.RegistrationNumber;
                Color = vehicle.Color;
                Brand = vehicle.Brand;
                Model = vehicle.Model;
                NumberOfWheels = vehicle.NumberOfWheels;

                if (vehicle.Owner != null)
                {
                    OwnerFirstName = vehicle.Owner.FirstName;
                    OwnerLastName = vehicle.Owner.LastName;
                }

                if (vehicle.VehicleType != null)
                {
                    VehicleTypeName = vehicle.VehicleType.TypeName;
                }

                // You can modify this logic based on how you want to select the parking record
                var parkingRecord = vehicle.ParkingRecords?.LastOrDefault();
                if (parkingRecord != null)
                {
                    ParkTime = parkingRecord.ParkTime;
                    CheckOutTime = parkingRecord.CheckOutTime;
                    FormattedParkingDuration = GetFormattedParkingTime(parkingRecord);
                }
            }
        }

        public VehicleDetailedViewModel()
        {
        }

        public string GetFormattedParkingTime(ParkingRecord parkingRecord)
        {
            // Calculate duration based on whether the vehicle has been checked out
            var duration = (parkingRecord.CheckOutTime ?? DateTime.Now) - parkingRecord.ParkTime;
            return duration.Days > 0
                ? $"{duration.Days} days, {duration.Hours} hours, {duration.Minutes} minutes"
                : $"{duration.Hours} hours, {duration.Minutes} minutes";
        }
    }
}

/* This loop will create a table row for each vehicle with a "View Details" link that passes the vehicle's ID to the Details action of VehicleController.
*
* @foreach (var vehicle in Model.Vehicles)
*{
*    <tr>
*        <td>@vehicle.RegistrationNumber</td>
*        // Other vehicle properties
*        <td>
*            <a asp-action="Details" asp-route-id="@vehicle.VehicleID">View Details</a>
*        </td>
*    </tr>
*}
*
 */