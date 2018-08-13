using System.ComponentModel.DataAnnotations;

namespace AyerLechon.Api.Models
{
    public class ChangePasswordViewModel
    {
        [MinLength(6)]
        [Required]
        public string CurrentPassword { get; set; }
        [MinLength(6)]
        [Required]
        public string NewPassword { get; set; }
    }
}