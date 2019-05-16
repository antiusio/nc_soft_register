using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataBase;
using Proxy;

namespace OpenTunnels
{
    class Program
    {
        public static List<SocksSsh> listSocks;
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            listSocks = new List<SocksSsh>();
            //Repair.RepairDB();
            object lockObj = new object();
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                //foreach (var t in ncSoftBase.open_socks_tunnels)
                //{
                //    t.status_observing = "Not opened";
                //}
                //ncSoftBase.SaveChanges();
                var proxys = ncSoftBase.proxys.ToList();
                for (; ; )
                {
                    bool allClosed = true;
                    foreach (var t in ncSoftBase.open_socks_tunnels)
                    {
                        
                        if (t.status_defiant.Equals("Open") && t.status_observing.Equals("Not opened"))
                        {
                            t.status_observing = "Opening";
                            ncSoftBase.SaveChanges();
                            Task.Run(()=> 
                            {
                                proxy proxy = null;
                                proxy = proxys.Where(p => p.id == t.proxy_id).First();
                                SocksSsh socksP = null;
                                try
                                {
                                    socksP = new SocksSsh(proxy);
                                    Console.WriteLine("SSH tunnel " + proxy.ip + " opened, port = " + socksP.Port);
                                    listSocks.Add(socksP);
                                    t.local_port = (int)socksP.Port;
                                    t.status_observing = "Opened";
                                    ncSoftBase.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("SSH tunnel " + proxy.ip + " not opened ");
                                    t.status_observing = "Not opens";
                                    ncSoftBase.SaveChanges();
                                    //continue;
                                }


                            });
                        }
                        if (t.status_defiant.Equals("Close") && t.status_observing.Equals("Opened"))
                        {
                            //var socksP = new SocksSsh(ncSoftBase.proxys.Where(p => p.id.Equals(t.proxy_id)).First());
                            listSocks.Where(s => s.Port.Equals(t.local_port)).First().Dispose();
                            Console.WriteLine("SSH tunnel closed, port = " + t.local_port.ToString());
                            t.status_observing = "Closed";
                            ncSoftBase.SaveChanges();
                        }
                        if (!t.status_observing.Equals("Closed"))
                            allClosed = false;
                    }
                    if (allClosed && ncSoftBase.open_socks_tunnels.Count()!=0)
                    {
                        //listSocks
                        Console.WriteLine("DataDirectory = " + Directory.GetCurrentDirectory());
                        var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["NcSoftBase"].ConnectionString;
                        Console.WriteLine("Connection string = "+ connectionString);
                        int n = ncSoftBase.open_socks_tunnels.Count();
                        Console.WriteLine("Count = "+ n.ToString());
                        Console.WriteLine("Press any key");
                        Console.ReadKey();
                        return;
                    }
                    Thread.Sleep(60000);
                }
                
                
            }
            
        }
    }
}
