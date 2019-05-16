namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class account
    {
        public int id { get; set; }

        public int email_id { get; set; }

        [StringLength(16)]
        public string password_ { get; set; }

        [StringLength(16)]
        public string display_name { get; set; }

        public DateTime date_of_birth { get; set; }

        public int proxy_id { get; set; }

        public int? status_id { get; set; }

        public DateTime date_created { get; set; }

        public DateTime? date_registered { get; set; }

        public DateTime? date_confirmed { get; set; }

        public int? count_try { get; set; }

        public int? user_agent_id { get; set; }

        public virtual email email { get; set; }

        public virtual proxy proxy { get; set; }

        public virtual statuses_registration statuses_registration { get; set; }

        public virtual user_agents user_agents { get; set; }
    }
}
