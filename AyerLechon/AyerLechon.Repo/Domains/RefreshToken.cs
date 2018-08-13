namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RefreshToken
    {
        [Key]
        public string TokenID { get; set; }

        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientId { get; set; }

        public long IssuedUtc { get; set; }

        public long ExpiresUtc { get; set; }

        [Required]
        public string ProtectedTicket { get; set; }
    }
}
