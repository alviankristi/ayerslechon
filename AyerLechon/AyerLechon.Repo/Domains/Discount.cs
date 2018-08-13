namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Discount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Discount()
        {
            OrderSummaries = new HashSet<OrderSummary>();
        }

        public int DiscountID { get; set; }

        public int Image { get; set; }

        public long ExpiredDate { get; set; }

        public int? ItemID { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        public double? Percentage { get; set; }

        public virtual FileStorage FileStorage { get; set; }

        public virtual Item Item { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderSummary> OrderSummaries { get; set; }
    }
}
