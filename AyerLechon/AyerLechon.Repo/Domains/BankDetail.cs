namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BankDetail
    {
        public int BankDetailId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
