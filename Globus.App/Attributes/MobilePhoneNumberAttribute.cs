using Globus.App.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Globus.App.Attributes
{
    public class MobilePhoneNumberAttribute : ValidationAttribute
    {
        private MnoCodeService _mnoCodeService;
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            ValidationResult validationResult = null;

            if (value == null)
            {
                validationResult = new ValidationResult("A valid mobile number is required");
                return validationResult;
            }

            string input = (string)value;
            
            if(!long.TryParse(input, out _))
            {
                validationResult = new ValidationResult("A mobile number should contain only digits");
                return validationResult;
            }

            if (input.Length != 11)
            {
                validationResult = new ValidationResult("A valid mobile number contains 11 digits");
                return validationResult;
            }

            _mnoCodeService = validationContext.GetRequiredService<MnoCodeService>();
            if (!_mnoCodeService.IsValidMnoCode(input.Substring(0, 4)))
            {
                validationResult = new ValidationResult("A mobile number should begin with a valid mobile network operator code");
                return validationResult;
            }
            return validationResult;
        }
    }
}
