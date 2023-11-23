using System.ComponentModel.DataAnnotations;

namespace Garage3.ViewModels
{
    public class VehicleRetrieveViewModel
    {
        private string? _registrationNumber;
        [MaxLength(10)]
        [RegularExpression("^[A-Z0-9]*$", ErrorMessage = "A valid registration number is required.")]
        public string? RegistrationNumber
        {
            get => _registrationNumber;

            set => _registrationNumber = value?.ToUpper();
        }
        public bool PrintReceipt { get; set; }

    }
}
