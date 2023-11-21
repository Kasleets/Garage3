using System.ComponentModel.DataAnnotations;

namespace Garage3.ViewModels
{
    public class UnparkViewModel
    {
        private string? _registrationNumber;
       
        public string? RegistrationNumber
        {
            get => _registrationNumber;

            set => _registrationNumber = value?.ToUpper();
        }
    }
}
