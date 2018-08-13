namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Region
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Region()
        {
            Customers = new HashSet<Customer>();
            OrderSummaries = new HashSet<OrderSummary>();
        }

        public int RegionID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public double Distance { get; set; }

        [Column(TypeName = "money")]
        public decimal DeliveryFee { get; set; }

        public bool ShowOnApp { get; set; }

        public bool IsPickupAtStore { get; set; }

        public bool IsAirFreight { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderSummary> OrderSummaries { get; set; }
    }
}
