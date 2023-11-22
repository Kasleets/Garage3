namespace Garage3.Models.Entities
{
    public class Member
    {
        public int MemberID { get; set; }
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; }
    }

}




//public string FullName => $"{FirstName} {LastName}";