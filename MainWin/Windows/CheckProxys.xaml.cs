using DataBase;
using Microsoft.Win32;
using Proxy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWin.Windows
{
    /// <summary>
    /// Interaction logic for CheckProxys.xaml
    /// </summary>
    public partial class CheckProxysW : Window
    {
        IEnumerable<MyDataContextMainWin.Proxy> Proxys;
        MyDataContext myDataContext;
        public CheckProxysW(IEnumerable<MyDataContextMainWin.Proxy> Proxys)
        {
            this.Proxys = Proxys;
            InitializeComponent();
            myDataContext = new MyDataContext(Proxys);
            DataContext = myDataContext;
            StartCheck();
        }
        public Task StartCheck()
        {
            return myDataContext.CheckProxys();
        }
        public class Proxy : INotifyPropertyChanged, Report
        {
            public proxy ProxyDB;
            public Proxy(proxy p)
            {
                ProxyDB = p;
                Ip = p.ip;
                Port = p.port;
            }
            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
            private string ip;
            public string Ip
            {
                get { return ip; }
                set { ip = value; OnPropertyChanged("Ip"); }
            }
            private int port;
            public int Port
            {
                get { return port; }
                set { port = value; OnPropertyChanged("Port"); }
            }
            private int percentage;
            public int Percentage
            {
                get { return percentage; }
                set { percentage = value; OnPropertyChanged("Percentage"); }
            }
            private string status;
            public string Status
            {
                get { return status; }
                set { status = value; OnPropertyChanged("Status"); }
            }
        }
        
        public class MyDataContext : INotifyPropertyChanged
        {
            private ObservableCollection<Proxy> proxys;
            public ObservableCollection<Proxy> Proxys
            {
                get { return proxys; }
                set { proxys = value; OnPropertyChanged("Proxys"); }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
            IEnumerable<MyDataContextMainWin.Proxy> ProxysDB;
            public MyDataContext(IEnumerable<MyDataContextMainWin.Proxy> Proxys)
            {
                ProxysDB = Proxys;
                this.Proxys = new ObservableCollection<Proxy>();
                foreach (var p in Proxys)
                {
                    this.Proxys.Add( new Proxy(p.proxyDB) );
                }
            }
            public Task CheckProxys()
            {
                //Task<string>[] tasks = new Task<string>[Proxys.Count];
                return Task.Run(() =>
                {
                    Task<string>[] tasks = new Task<string>[Proxys.Count()];
                    int index = 0;
                    foreach (var p in ProxysDB)
                    {
                        Proxys[index].Percentage = 10;
                        Proxys[index].Status = "Проверка начата";
                        var lp = Proxys[index];
                        tasks[index] = Task.Run(() => { return SSHProxy.Check(p.proxyDB, lp); });
                        
                        index++;
                    }
                    Task.WaitAll(tasks);
                    index = 0;
                    //foreach(var t in tasks)
                    //{
                    //    Proxys[index].Status = t.Result;
                    //    Proxys[index].Percentage = 100;
                    //    index++;
                    //}
                });
            }
            public void SaveToFile(string filePath)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var p in Proxys)
                {
                    if(!p.Status.Equals("All right"))
                    {
                        sb.Append(p.Ip);
                        sb.Append(':');
                        sb.Append(p.Port);
                        sb.Append(':');
                        sb.Append(p.ProxyDB.login_);
                        sb.Append(':');
                        sb.Append(p.ProxyDB.password_);
                        sb.Append(':');
                        sb.Append(p.Status);
                        sb.Append("\r\n");
                    }
                }
                sb.Append("\r\n\r\n\r\n");
                foreach (var p in Proxys)
                {
                    if (p.Status.Equals("All right"))
                    {
                        sb.Append(p.Ip);
                        sb.Append(':');
                        sb.Append(p.Port);
                        sb.Append(':');
                        sb.Append(p.ProxyDB.login_);
                        sb.Append(':');
                        sb.Append(p.ProxyDB.password_);
                        sb.Append(':');
                        sb.Append(p.Status);
                        sb.Append("\r\n");
                    }
                }
                File.WriteAllText(filePath, sb.ToString());
            }

        }

        private void InFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.OverwritePrompt = true;
            sfd.InitialDirectory = "Desktop";
            sfd.Filter = "Text File | *.txt";
            sfd.FileName = "CheckedProxys.txt";
            bool? r = sfd.ShowDialog();
            if (r == true)
            {
                myDataContext.SaveToFile(sfd.FileName);
            }
        }
    }
}
