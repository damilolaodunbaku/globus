using Globus.App.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Business.Services
{
    public class OtpService
    {
        private readonly ILogger<OtpService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly Random _random;
        public OtpService(ILogger<OtpService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _random = new Random();
        }

        // Simplistic generation of an OTP.
        public string GenerateOtp()
        {
            //return _random.Next(999999).ToString();
            return "123456";
        }

        public bool CheckCustomerValidationStatus(string emailAddress,string mobileNumber)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                return unitOfWork.OTPS.IsMobileNumberValidated(emailAddress, mobileNumber);
            }
        }
    }

    public class OtpValidationResult
    {
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; }
    }
}
