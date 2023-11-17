namespace Garage3.Models.Entities
{
#nullable disable
    public class Vehicle
    {
        public int Id { get; set; }
        
        public string RegNum { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        // Todo: Foreign Key to the Customer table

        // Todo: Navigation property to the Garage?
    }
}
