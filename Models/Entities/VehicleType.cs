namespace Garage3.Models.Entities
{
    public class VehicleType
    {
        public int VehicleTypeID { get; set; }
        public string? TypeName { get; set; }

        // Navigation property
        public virtual ICollection<Vehicle>? Vehicles { get; set; }
    }

}
