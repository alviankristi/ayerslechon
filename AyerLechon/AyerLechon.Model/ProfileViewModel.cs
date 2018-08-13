using System.ComponentModel.DataAnnotations;

namespace AyerLechon.Model
{
    public class ProfileViewModel
    {
        [Required]
        public int? Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string Address { get; set; }
        public int? RegionId { get; set; }
        public string PhoneNumber { get; set; }
        public bool NewVipApplication { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
