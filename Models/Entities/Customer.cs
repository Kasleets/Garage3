namespace Garage3.Models.Entities
{
    public class Customer
    {
#nullable disable
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public int PersonNummer { get; set; }

        // Note: Could calculate age from PersonNummer
        public int Age { get; set; }

        // Note: Inspired by Dimitris project 
        // Todo: Navigation property to Vehicle?
        // Todo: Conv 2 same as Conv 1?
    }
}
