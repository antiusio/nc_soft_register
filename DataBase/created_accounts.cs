namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class created_accounts
    {
        [StringLength(50)]
        public string status_ { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(16)]
        public string password_ { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string ip { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int port { get; set; }

        public int? count_try { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime date_created { get; set; }

        public DateTime? date_registered { get; set; }

        public DateTime? date_confirmed { get; set; }
    }
}
