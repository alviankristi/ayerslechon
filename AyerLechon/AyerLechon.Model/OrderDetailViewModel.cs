using System.ComponentModel.DataAnnotations;

namespace AyerLechon.Model
{
    public class OrderDetailViewModel
    {
        [Required]
        public int ItemId { get; set; }
        public ProductViewModel Product { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
