using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public static class Repair
    {
        public static void RepairDB()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NcSoftBase"].ConnectionString;
            SqlCeEngine engine =
             new SqlCeEngine(connectionString);
            if (false == engine.Verify())
            {
                Console.WriteLine("Database is corrupted.");
                try
                {
                    engine.Repair(null, RepairOption.RecoverAllPossibleRows);
                }
                catch (SqlCeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                if (ncSoftBase.settings.Count() == 0)
                {
                    ncSoftBase.settings.Add(new setting() { captcha_api_key = "d2046ad540a4366975d2893791dc6d9e", count_threads=1 });
                    ncSoftBase.SaveChanges();
                }
                if (ncSoftBase.types_proxy.Count() == 0)
                {
                    ncSoftBase.types_proxy.Add(new types_proxy() { text_type="ssh" });
                    ncSoftBase.types_proxy.Add(new types_proxy() { text_type = "https" });
                    ncSoftBase.types_proxy.Add(new types_proxy() { text_type = "socks" });
                    ncSoftBase.SaveChanges();
                }
                if (ncSoftBase.countrys.Count() == 0)
                {
                    ncSoftBase.countrys.Add(new country() { country1 = "anywhere" });
                    ncSoftBase.SaveChanges();
                }
                if (ncSoftBase.statuses_registration.Count() == 0)
                {
                    ncSoftBase.statuses_registration.Add(new statuses_registration() { text_status = "created" });
                    ncSoftBase.statuses_registration.Add(new statuses_registration() { text_status = "registered" });
                    ncSoftBase.statuses_registration.Add(new statuses_registration() { text_status = "confirmed" });
                    ncSoftBase.SaveChanges();
                }
                if (ncSoftBase.open_socks_tunnels.Count() != 0)
                {
                    ncSoftBase.open_socks_tunnels.RemoveRange(ncSoftBase.open_socks_tunnels);
                    ncSoftBase.SaveChanges();
                }
                if (ncSoftBase.results_using_proxy.Count() == 0)
                {
                    ncSoftBase.results_using_proxy.Add(new results_using_proxy() { text = "error" });
                    ncSoftBase.results_using_proxy.Add(new results_using_proxy() { text = "registered" });
                    ncSoftBase.SaveChanges();
                }
                
                //if (ncSoftBase.open_socks_tunnels.Count() == 0)
                //{
                //    //ncSoftBase.proxys.Add(new proxy() { ip = "80.106.240.243", port = 2222, login_ = "ubnt", password_ = "ubnt" });
                //    ncSoftBase.open_socks_tunnels.Add(new open_socks_tunnels() { proxy_id = 1, status_defiant = "Open", status_observing = "Not opened" });
                //    ncSoftBase.SaveChanges();
                //}
            }
        }
    }
}
