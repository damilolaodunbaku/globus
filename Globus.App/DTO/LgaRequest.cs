using System.ComponentModel.DataAnnotations;

namespace Globus.App.DTO
{
    public class LgaRequest
    {
        [Required]
        public string State { get; set; }
    }
}
