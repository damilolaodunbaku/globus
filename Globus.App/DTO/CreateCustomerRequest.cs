using Globus.App.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Globus.App.DTO
{
    public class CreateCustomerRequest
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [MobilePhoneNumber]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string LGA { get; set; }
    }
}
