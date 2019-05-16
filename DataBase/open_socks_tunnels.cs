namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class open_socks_tunnels
    {
        public int id { get; set; }

        public int? proxy_id { get; set; }

        [StringLength(50)]
        public string status_defiant { get; set; }

        [StringLength(50)]
        public string status_observing { get; set; }

        public int? local_port { get; set; }

        public virtual proxy proxy { get; set; }
    }
}
