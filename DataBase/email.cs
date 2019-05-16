namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class email
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public email()
        {
            accounts = new HashSet<account>();
        }

        public int id { get; set; }

        [Column("email")]
        [StringLength(50)]
        public string email1 { get; set; }

        [StringLength(16)]
        public string password_ { get; set; }

        public int? country_id { get; set; }

        [StringLength(50)]
        public string confirm_email { get; set; }

        [StringLength(11)]
        public string phone_confirm_email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<account> accounts { get; set; }

        public virtual country country { get; set; }
    }
}
