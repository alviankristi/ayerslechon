namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public int OrderSummaryID { get; set; }

        public int ItemID { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        [Column(TypeName = "money")]
        public decimal SubTotal { get; set; }

        public virtual Item Item { get; set; }

        public virtual OrderSummary OrderSummary { get; set; }
    }
}
