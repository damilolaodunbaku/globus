using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Data.Entities
{
    public class OTP
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        public string RecipientEmailAddress { get; set; }
        [Required]
        [StringLength(11)]
        public string RecipientMobileNumber { get; set; }
        [Required]
        public string HashedOtp { get; set; }
        public DateTime RequestTime { get; set; }
        public bool IsValidatedSuccessfully { get; set; }
        public DateTime? ValidationTime { get; set; }
    }
}
