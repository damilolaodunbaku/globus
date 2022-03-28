using Globus.App.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Globus.App.DTO
{
    public class ValidateOtpRequest : IValidatableObject
    {
        [Required]
        public Guid MessageReference { get; set; }
        [Required]
        [StringLength(6)]
        public string OTP { get; set; }
        [MobilePhoneNumber]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if(!long.TryParse(OTP, out _))
            {
                validationResults.Add(new ValidationResult("OTP should contain digits only",
                    new List<string> { nameof(OTP) }));
            }

            return validationResults;
        }
    }
}
