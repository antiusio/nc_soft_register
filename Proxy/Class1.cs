using DataBase;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proxy
{
    
    public class SshTunnel : IDisposable
    {
        private SshClient client;
        private ForwardedPortDynamic port;
        public uint? Port
        {
            get
            {
                if (!(port is null))
                    return port.BoundPort;
                return null;
            }
        }
        public SshTunnel(ConnectionInfo connectionInfo)
        {
            
            try
            {
                client = new SshClient(connectionInfo);
                client.Connect();
                
                Random r = new Random((int)DateTime.Now.ToBinary());
                for(; ; )
                    try
                    {
                            port = new ForwardedPortDynamic("127.0.0.1", (uint)r.Next(1, 65535));

                        client.AddForwardedPort(port);
                        port.Start();
                        break;
                    }
                    catch
                    {

                    }



                //if (client.IsConnected && port.IsStarted)
                //{
                //    MessageBox.Show("Connected");
                //}
                //System.Threading.Thread.Sleep(20 * 1000);
                // HACK to get the dynamically allocated client port
                //var listener = (TcpListener)typeof(ForwardedPortLocal).GetField("_listener", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(port);
                //localPort = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            }
            catch(Exception e)
            {
                Dispose();
                throw;
            }
           
        }

        //public int LocalPort { get { return localPort; } }

        public void Dispose()
        {
            if (port != null)
                port.Dispose();
            if (client != null)
                client.Dispose();
        }
    }
    public class SocksSsh : IDisposable
    {
        private SshTunnel sshTunel = null;
        private proxy p;
        public uint? Port
        {
            get { if (sshTunel != null)
                    return sshTunel.Port;
                else
                    return null;
            }
        }
        public SocksSsh(proxy p)
        {
            this.p = p;
            ConnectionInfo connectionInfo = new ConnectionInfo(p.ip, p.port, p.login_,
                new AuthenticationMethod[]
                { // Pasword based Authentication
                    new PasswordAuthenticationMethod(p.login_,p.password_)
                }
                );
            try
            {
                sshTunel = new SshTunnel(connectionInfo);
            }
            catch(Exception ex)
            { }
            
        }
        public void Dispose()
        {
            sshTunel.Dispose();
        }
    }
    public interface Report
    {
        int Percentage { get; set; }
        string Status { get; set; }
    }
    public static class SSHProxy
    {
        public static string Check(proxy p, Report rep)
        {
            
            SshClient client;
            ConnectionInfo connectionInfo = new ConnectionInfo(p.ip, p.port, p.login_,
                new AuthenticationMethod[]
                { // Pasword based Authentication
                    new PasswordAuthenticationMethod(p.login_,p.password_)
                }
                );
            client = new SshClient(connectionInfo);
            try
            {
                rep.Status = "Try to connect";
                rep.Percentage = 50;
                client.Connect();
            }
            catch (Exception ex)
            {
                rep.Status = ex.Message;
                rep.Percentage = 100;
                return ex.Message;
            }
            rep.Status = "All right";
            rep.Percentage = 100;
            client.Disconnect();
            client.Dispose();
            return "AllRight";
            
        }

    }
}
