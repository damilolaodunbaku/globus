using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Data.Entities
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        [Required]
        public string HashedPassword { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string LGA { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
