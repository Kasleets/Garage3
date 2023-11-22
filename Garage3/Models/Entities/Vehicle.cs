using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.Entities
{
#nullable disable
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public int OwnerID { get; set; }

        private string _registrationNumber;
        [Required]
        [MaxLength(10)]
        [RegularExpression("^[A-Z0-9]*$", ErrorMessage = "Registration number is required. Max 10 signs.")]
        public string RegistrationNumber { 
            get => _registrationNumber; 
            set => _registrationNumber = value.ToUpper(); 
        }

        public int VehicleTypeID { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int NumberOfWheels { get; set; }

        // Navigation properties
        public virtual Member Owner { get; set; }
        public virtual VehicleType VehicleType { get; set; }
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; }
    }

}
