namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderStatu
    {
        [Key]
        public int OrderStatusID { get; set; }

        [StringLength(50)]
        public string OrderStatus { get; set; }
    }
}
