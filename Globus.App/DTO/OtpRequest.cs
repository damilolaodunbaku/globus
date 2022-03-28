using Globus.App.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Globus.App.DTO
{
    public class OtpRequest
    {
        [EmailAddress]
        [StringLength(70)]
        public string EmailAddress { get; set; }
        [MobilePhoneNumber]
        public string MobileNumber { get; set; }
    }
}
