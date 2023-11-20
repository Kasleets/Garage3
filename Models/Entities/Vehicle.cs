namespace Garage3.Models.Entities
{
#nullable disable
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public int OwnerID { get; set; }
        public string RegistrationNumber { get; set; }
        public int VehicleTypeID { get; set; }

        // Navigation properties
        public virtual Member Owner { get; set; }
        public virtual VehicleType VehicleType { get; set; }
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; }
    }

}
