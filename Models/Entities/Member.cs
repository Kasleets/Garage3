using System.Security.Policy;

namespace Garage3.Models.Entities
{
    public class Member
    {
#nullable disable
        public int MemberID { get; set; }
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        // Add membership type by Xiahui
        public MembershipType MembershipType { get; set; }

        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; }
    }

    public enum MembershipType // add enum membership type by Xiahui
    {
        Regular,
        Silver,
        Gold,
        VIP
    
    }

}





//public string FullName => $"{FirstName} {LastName}";