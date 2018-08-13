namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PaymentOption
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PaymentOption()
        {
            OrderSummaries = new HashSet<OrderSummary>();
        }

        public int PaymentOptionID { get; set; }

        [Column("PaymentOption")]
        [Required]
        public string PaymentOption1 { get; set; }

        public double? Charge { get; set; }

        public string BankDetailLine1 { get; set; }

        public string BankDetailLine2 { get; set; }

        public string BankDetailLine3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderSummary> OrderSummaries { get; set; }
    }
}
