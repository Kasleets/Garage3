using AutoMapper.Execution;
using Garage3.Models.Entities;


namespace Garage3.ViewModels
{
    public class ParkingRecordView
    {
#nullable disable
        public int ParkingRecordID { get; set; }
        public int VehicleID { get; set; }
        public DateTime ParkTime { get; set; }
        public DateTime? CheckOutTime { get; set; } // Add this property for recording checkout time of vehicles

        public int MemberID { get; set; }

        // Navigation properties
        public virtual Vehicle Vehicle { get; set; }
        public virtual AutoMapper.Execution.Member Member { get; set; }

        public TimeSpan GetParkingTime()
        {
            return CheckOutTime.HasValue ? CheckOutTime.Value - ParkTime : DateTime.Now - ParkTime;
        }

        // Todo: need to figure out PricePerHour
        //public double GetParkingCost()
        //{
        //    return GetParkingTime().TotalHours * Vehicle.VehicleType.PricePerHour;
        //}

        // During the instance of the Parking, set a timestamp for ParkTime
    }
}
