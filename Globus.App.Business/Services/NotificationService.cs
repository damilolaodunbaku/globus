using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Business.Services
{
    /// <summary>
    /// Responsible for sending an OTP to a customer's email and/or mobile.
    /// </summary>
    public class NotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Perform no operation, effectively a mock.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="mobileNumber"></param>
        /// <param name="OTP"></param>
        public void SendOtpToCustomer(string emailAddress,string mobileNumber,string OTP)
        {

        }
    }
}
