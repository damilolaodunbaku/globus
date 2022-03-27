using Globus.App.Data.Repositories.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly Random _random;
        public OtpService(ILogger<OtpService> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _random = new Random();
        }

        // Simplistic generation of an OTP.
        public string GenerateOtp()
        {
            return _random.Next(999999).ToString();
        }

        public bool CheckCustomerValidationStatus(string emailAddress,string mobileNumber)
        {
            return _unitOfWork.OTPS.IsMobileNumberValidated(emailAddress, mobileNumber);
        }
        //public OtpValidationResult ValidateOtp()
        //{

        //}

    }

    public class OtpValidationResult
    {
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; }
    }
}
