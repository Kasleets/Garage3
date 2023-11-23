using System;

public class GarageSettings
{
    public const string VehicleRates = "VehicleRates";

    public int Capacity { get; set; }

    public decimal CarHourlyRate { get; set; }
    public decimal MotorcycleHourlyRate { get; set; }
    public decimal TruckHourlyRate { get; set; }
    public decimal BusHourlyRate { get; set; }
    public decimal AirplaneHourlyRate { get; set; }
}
