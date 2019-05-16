using DataBase;
using Microsoft.Win32;
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
    /// Interaction logic for ConfirmedAccs.xaml
    /// </summary>
    public partial class AccsW : Window
    {
        MyDataContext myDataContext = null;
        int n;
        public AccsW(int n=0)
        {
            this.n = n;
            InitializeComponent();
            myDataContext = new MyDataContext(n);
            DataContext = myDataContext;
        }

        private void ExportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.OverwritePrompt = true;
            sfd.InitialDirectory = "Desktop";
            sfd.Filter = "Text File | *.txt";
            if(n==0)
                sfd.FileName = "AllAccs.txt";
            if (n == 1)
                sfd.FileName = "CreatedAccs.txt";
            if (n == 2)
                sfd.FileName = "RegisteredAccs.txt";
            if (n == 3)
                sfd.FileName = "ConfirmedAccs.txt";
            bool? r = sfd.ShowDialog();
            if (r == true)
            {
                myDataContext.SaveToFile(sfd.FileName);
            }
        }
        public class MyDataContext : INotifyPropertyChanged
        {
            ObservableCollection<created_accounts> listAccs = null;
            public ObservableCollection<created_accounts> ListAccs
            {
                get { return listAccs; }
                set { listAccs = value;OnPropertyChanged("ListAccs"); }
            }
            public MyDataContext(int n = 0)
            {
                ListAccs = new ObservableCollection<created_accounts>();
                using (var ncSoftBase = new NcSoftBase())
                {
                    
                    foreach(var acc in ncSoftBase.accounts)
                    {
                        if(n==0||(n!=0&&acc.status_id == n))
                        {
                            created_accounts accCreated = new created_accounts() { count_try = acc.count_try, date_confirmed = acc.date_confirmed, date_created = acc.date_created, password_ = acc.password_, email = acc.email.email1, ip = acc.proxy.ip, port= acc.proxy.port, status_ = acc.statuses_registration.text_status };
                            ListAccs.Add(accCreated);
                        }
                        //ncSoftBase.SaveChanges();
                        //break;
                    }
                    foreach (var acc in ncSoftBase.created_accounts)
                    {
                        ListAccs.Add(acc);
                    }
                    foreach (var acc in ncSoftBase.accounts)
                    {
                        if (acc.status_id == 3)
                        {
                            foreach (var stat in ncSoftBase.statuses_registration)
                            {

                            }
                        }
                    }
                }


            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop="")
            {
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prop));
            }
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    foreach (var acc in listAccs)
                    {
                        sb.Append(acc.email);
                        sb.Append(':');
                        sb.Append(acc.password_);
                        sb.Append(':');
                        sb.Append(acc.status_);
                        sb.Append(':');
                        sb.Append(ncSoftBase.emails.Where(x=>x.email1.Equals(acc.email)).First().password_);
                        sb.Append(':');
                        sb.Append(ncSoftBase.emails.Where(x => x.email1.Equals(acc.email)).First().confirm_email);
                        if (!(acc.count_try is null))
                        {
                            sb.Append(':');
                            sb.Append(acc.count_try);
                        }
                        sb.Append('\r');
                        sb.Append('\n');
                    }
                    return sb.ToString();
                }
            }
            public void SaveToFile(string path)
            {
                //File.CreateText(path);
                File.WriteAllText(path,ToString());
            }
        }
    }
}
