namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class results_using_proxy
    {
        public int id { get; set; }

        [StringLength(20)]
        public string text { get; set; }
    }
}
