using Globus.App.Business.Services;
using Globus.App.Data.Entities;
using Globus.App.Data.Repositories.Interfaces;
using Globus.App.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Globus.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly ILogger<OTPController> _logger;
        private readonly EncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationService _notificationService;
        private readonly OtpService _otpService;

        public OTPController(ILogger<OTPController> logger,
            NotificationService notificationService,
            EncryptionService encryptionService,
            IUnitOfWork unitOfWork,
            OtpService otpService)
        {
            _logger = logger;
            _encryptionService = encryptionService;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _otpService = otpService;
        }

        [HttpPost]
        public ActionResult SendOTP([FromBody] OtpRequest request)
        {
            if (_otpService.CheckCustomerValidationStatus(request.EmailAddress, request.MobileNumber))
            {
                return Problem("Email address or Phone number has already been validated successfully",
                               statusCode:StatusCodes.Status400BadRequest,
                               title:"Bad request");
            }

            string otpCode = _otpService.GenerateOtp();
            string hashedOtpCode = _encryptionService.GetHash(otpCode);

            _notificationService.SendOtpToCustomer(request.EmailAddress, request.MobileNumber, otpCode);

            OTP oTP = new OTP()
            {
                HashedOtp = hashedOtpCode,
                MessageReference = Guid.NewGuid(),
                RecipientEmailAddress = request.EmailAddress,
                RecipientMobileNumber = request.MobileNumber,
                RequestTime = DateTime.Now,
            };

            _unitOfWork.OTPS.Add(oTP);
            _unitOfWork.Complete();

            return new JsonResult(oTP.MessageReference);
        }
    }
}
