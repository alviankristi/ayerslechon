namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderSummary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderSummary()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderSummaryID { get; set; }

        public long OrderDate { get; set; }

        public long? DateNeeded { get; set; }

        public int CustomerID { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public int? RegionID { get; set; }

        public string DeliveryAddress { get; set; }

        [Column(TypeName = "money")]
        public decimal AmountPaid { get; set; }

        public int PaymentOptionId { get; set; }

        public string Notes { get; set; }

        public int? DiscountId { get; set; }

        public int? PaymentStatusId { get; set; }

        public int? OrderStatusID { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Discount Discount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual PaymentOption PaymentOption { get; set; }

        public virtual PaymentStatu PaymentStatu { get; set; }

        public virtual Region Region { get; set; }
    }
}
