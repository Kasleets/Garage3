using System;


    namespace Garage3.ViewModels
    {

        public class ReceiptViewModel
        {
    #nullable disable
            public int VehicleId { get; set; }
            public string RegistrationNumber { get; set; }
            public DateTime ArrivalTime { get; set; }
            public DateTime DepartureTime { get; set; }
            public TimeSpan ParkingTime { get; set; }
            public decimal CostPerHour { get; set; }

            public ReceiptViewModel(
                int vehicleId,
                string registrationNumber,
                DateTime arrivalTime,
                DateTime departureTime,
                decimal costPerHour)
            {
                VehicleId = vehicleId;
                RegistrationNumber = registrationNumber;
                ArrivalTime = arrivalTime;
                DepartureTime = departureTime;
                ParkingTime = departureTime - arrivalTime;
                CostPerHour = costPerHour;
            }

            public string FormattedTotalCost
            {
                get
                {
                    // Calculate the total cost using GarageSettings
                    decimal totalCost = (decimal)ParkingTime.TotalHours * CostPerHour;

                    // Round the total cost to the nearest whole number
                    totalCost = Math.Round(totalCost);

                    // Format the total cost with the currency symbol
                    string formattedCost = $"{totalCost:C0}";

                    return formattedCost;
                }
            }

            // Formatted parking time to display proper decimal places
            public string FormattedParkingTime
            {
                get
                {
                    string formattedTime = string.Empty;
                    if (ParkingTime.Days > 0)
                    {
                        formattedTime += $"{ParkingTime.Days:D1} days : ";
                    }

                    formattedTime += $"{ParkingTime.Hours:D1} hours : {ParkingTime.Minutes:D2} minutes : {ParkingTime.Seconds:D2} seconds";

                    return formattedTime;
                }
            }
        }


    }


