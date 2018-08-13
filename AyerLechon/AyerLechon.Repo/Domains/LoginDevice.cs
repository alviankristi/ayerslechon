namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LoginDevice
    {
        public int LoginDeviceID { get; set; }

        public string DeviceId { get; set; }

        public int CustomerID { get; set; }

        public long CreateDate { get; set; }

        public long? LastLoginDate { get; set; }

        [StringLength(50)]
        public string FbAccountId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
