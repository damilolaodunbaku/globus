using Globus.App.Business.Services;
using Globus.App.Data.Entities;
using Globus.App.Data.Repositories.Interfaces;
using Globus.App.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net.Mime;

namespace Globus.App.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError,"Internal server error")]
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
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation("Send an OTP to a customer","Sends a one-time password (OTP) to the provided email address and mobile number, for test purposes the generated OTP is 123456")]
        [SwaggerResponse(StatusCodes.Status200OK,"Returns a message reference which is used when validating the sent OTP",Type = typeof(Guid), ContentTypes = new string[] { MediaTypeNames.Application.Json })]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Attempted to validate a non-existing record")]

        public ActionResult SendOTP([FromBody] OtpRequest request)
        {
            try
            {
                if (_otpService.CheckCustomerValidationStatus(request.EmailAddress, request.MobileNumber))
                {
                    return Problem("Email address or Phone number has already been validated successfully",
                                   statusCode: StatusCodes.Status400BadRequest,
                                   title: "Bad request");
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
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("validate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation("Validate an OTP","Validate an OTP")]
        [SwaggerResponse(StatusCodes.Status200OK,"OTP Validated successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound,"Attempted to validate a non-existing record")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed")]
        public ActionResult ValidateOtp([FromBody] ValidateOtpRequest request)
        {
            try
            {
                OTP otp = _unitOfWork.OTPS.GetOtpByMessageReferece(request.MessageReference);

                if(otp == null)
                {
                    return Problem("No record found", title: "Not found", statusCode: StatusCodes.Status404NotFound);
                }

                if (otp.IsValidatedSuccessfully)
                {
                    return Problem("Record has been previously validated", title: "Bad request", statusCode: StatusCodes.Status400BadRequest);
                }

                // It's been more than 5 minutes, OTP has expired.
                if((DateTime.Now - otp.RequestTime) > TimeSpan.FromMinutes(5))
                {
                    return Problem("OTP has expired, please generate a new one", title: "Bad request", statusCode: StatusCodes.Status400BadRequest);
                }

                string otpHash = _encryptionService.GetHash(request.OTP);

                if (!otpHash.Equals(otp.HashedOtp))
                {
                    return Problem("Validation failed", title: "Bad request", statusCode: StatusCodes.Status400BadRequest);
                }

                otp.IsValidatedSuccessfully = true;
                otp.ValidationTime = DateTime.Now;

                _unitOfWork.Complete();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
