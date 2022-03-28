using Globus.App.Business.Services;
using Globus.App.Data.Repositories.Interfaces;
using Globus.App.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Globus.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly NigerianStatesService _nigerianStatesService;
        private readonly EncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly OtpService _otpService;

        public CustomerController(ILogger<CustomerController> logger,
            NigerianStatesService nigerianStatesService,
            EncryptionService EncryptionService,
            IUnitOfWork unitOfWork,
            OtpService otpService)
        {
            _logger = logger;
            _nigerianStatesService = nigerianStatesService;
            _encryptionService = EncryptionService;
            _unitOfWork = unitOfWork;
            _otpService = otpService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomersAsync(int pageNumber = 0, int pageSize = 10)
        {
            try
            {
                var customers = _unitOfWork.Customers.PageCustomers(pageNumber, pageSize);
                return new JsonResult(customers);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            try
            {
                if (!_nigerianStatesService.IsValidState(request.State))
                {
                    return Problem("An invalid nigerian state was provided",
                           title: "Bad request",
                           statusCode: StatusCodes.Status400BadRequest);
                }

                if (!_nigerianStatesService.IsValidLgaForState(request.State, request.LGA))
                {
                    return Problem("Provided LGA does not match the supplied state",
                           title: "Bad request",
                           statusCode: StatusCodes.Status400BadRequest);
                }

                if (!_otpService.CheckCustomerValidationStatus(request.EmailAddress, request.PhoneNumber))
                {
                    return Problem("Cannot onboard customer if phone number or email address has not been validated",
                           title: "Bad request",
                           statusCode: StatusCodes.Status400BadRequest);
                }

                _unitOfWork.Customers.Add(new Data.Entities.Customer
                {
                    PhoneNumber = request.PhoneNumber,
                    EmailAddress = request.EmailAddress,
                    CreationDateTime = DateTime.Now,
                    HashedPassword = _encryptionService.GetHash(request.Password),
                    State = request.State,
                    LGA = request.LGA,
                });

                _unitOfWork.Complete();

                return Ok();

            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "An error occured");
                return Problem(detail: $"A customer with the email address {request.EmailAddress} or phone number {request.PhoneNumber} already exists",
                        statusCode: StatusCodes.Status409Conflict,
                        title: "Duplicate request");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
