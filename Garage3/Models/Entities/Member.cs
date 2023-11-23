using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.Entities
{
#nullable disable
    public class Member
    {
        public int MemberID { get; set; }

        [Required]
        [Display(Name = "Social Security Number")]
        public string PersonalNumber { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(20), MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(20), MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [Range(18, 120)]
        [Display(Name = "Age")]
        public int Age { get; set; }
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; }
    }

}




//public string FullName => $"{FirstName} {LastName}";