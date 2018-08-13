using System.ComponentModel.DataAnnotations;

namespace AyerLechon.Api.Models
{
    public class RegisterViewModel
    {
        public string Address { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int? IDRegion { get; set; }
    }
}