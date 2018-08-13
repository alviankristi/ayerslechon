namespace AyerLechon.Repo.Domains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FileStore
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FileStoreId { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
