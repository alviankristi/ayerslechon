using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AyerLechon.Model
{
    public class OrderSummaryViewModel
    {
        [Required]
        public double? Amount { get; set; }

        public int OrderSummaryId { get; set; }

        [Required]
        [Display(Name = "Date and Time Needed")]
        public long? OrderDate { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int? CustomerId { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }

        [Display(Name = "Voucher Code")]
        public string DiscountCode { get; set; }

        [Required]
        [Display(Name = "Payment Option")]
        public int? PaymentOptionId { get; set; }

        public string Notes { get; set; }

        [Required]
        [Display(Name = "Region")]
        public int? RegionId { get; set; }

        [Required]
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
