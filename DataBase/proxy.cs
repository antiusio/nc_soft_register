namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("proxys")]
    public partial class proxy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public proxy()
        {
            accounts = new HashSet<account>();
            open_socks_tunnels = new HashSet<open_socks_tunnels>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(15)]
        public string ip { get; set; }

        public int port { get; set; }

        [StringLength(30)]
        public string login_ { get; set; }

        [StringLength(30)]
        public string password_ { get; set; }

        public int? type_id { get; set; }

        public int? country_id { get; set; }

        public DateTime? date_use { get; set; }

        public int? result_id { get; set; }

        [StringLength(50)]
        public string text_error { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<account> accounts { get; set; }

        public virtual country country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<open_socks_tunnels> open_socks_tunnels { get; set; }

        public virtual types_proxy types_proxy { get; set; }
    }
}
