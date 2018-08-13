namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            Discounts = new HashSet<Discount>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ItemID { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int? Image { get; set; }

        public int ReadyStock { get; set; }

        public int? CategoryID { get; set; }

        [Column(TypeName = "money")]
        public decimal WebPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal AirFreight { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Discount> Discounts { get; set; }

        public virtual FileStorage FileStorage { get; set; }

        public virtual ItemCategory ItemCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
