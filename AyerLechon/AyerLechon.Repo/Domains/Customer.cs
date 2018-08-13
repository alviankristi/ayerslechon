namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            LoginDevices = new HashSet<LoginDevice>();
            OrderSummaries = new HashSet<OrderSummary>();
        }

        public int CustomerID { get; set; }

        public string Address { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public Guid? ResetPasswordToken { get; set; }

        public string PhoneNumber { get; set; }

        public long RegisteredDate { get; set; }

        public long? LastLogin { get; set; }

        public long? LastChangePassword { get; set; }

        public long? LastResetPassword { get; set; }

        [Column(TypeName = "money")]
        public decimal CreditLimit { get; set; }

        public int? RegionID { get; set; }

        public bool VIP { get; set; }

        public bool NewVIPApplication { get; set; }

        public virtual Region Region { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoginDevice> LoginDevices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderSummary> OrderSummaries { get; set; }
    }
}
