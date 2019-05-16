using DataBase;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddProxys.xaml
    /// </summary>
    public partial class AddProxys : Window
    {
        public MyDataContext myDataContext = null;
        public AddProxys()
        {
            InitializeComponent();
            myDataContext = new MyDataContext();
            DataContext = myDataContext;
        }

        private void UploadSSHTunnelsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myDataContext.GetProxys();
            myDataContext.SaveProxys("ssh");
            DialogResult = true;
            Close();
        }
        public class MyDataContext : INotifyPropertyChanged
        {
            private string textProxys;
            public string TextProxys
            {
                get { return textProxys; }
                set { textProxys = value; }
            }
            public List<proxy> listProxys = null;

            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
            public List<proxy> GetProxys()
            {
                listProxys = new List<proxy>();
                if (textProxys == null)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    bool? rezDialog = openFileDialog.ShowDialog();
                    if (rezDialog == true)
                    {
                        textProxys = File.ReadAllText(openFileDialog.FileName);
                    }
                }
                if (textProxys is null)
                    return listProxys;
                var notEmptyStrings = textProxys.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in notEmptyStrings)
                {
                    var strings = s.Split(new char[] { ';' });
                    string ip = strings[0].Split(new char[] { ':' })[0];
                    int port = Convert.ToInt32(strings[0].Split(new char[] { ':' })[1]);
                    string login = strings[1];
                    string password = strings[2];
                    listProxys.Add(new proxy() { ip = ip, port = port, login_ = login, password_ = password });
                }
                return listProxys;
            }
            public void SaveProxys(string type)
            {
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    foreach (var proxy in listProxys)
                    {
                        var typeProxy = ncSoftBase.types_proxy.Where(p => p.text_type.Equals(type));
                        if (typeProxy.Count() != 0)
                        {
                            proxy.type_id = typeProxy.First().id;
                        }
                        proxy.country_id = 1;
                        if (ncSoftBase.proxys.Where(p => p.ip.Equals(proxy.ip)).Count() != 0)
                            proxy.id = ncSoftBase.proxys.Where(p => p.ip.Equals(proxy.ip)).First().id;

                        //var v = ncSoftBase.proxys.Where(p => p.ip.Equals(proxy.ip)).Count();
                        if (ncSoftBase.proxys.Where(p => p.ip.Equals(proxy.ip)).Count() != 0)
                        {
                            var proxyDb = ncSoftBase.proxys.Where(p => p.ip.Equals(proxy.ip)).First();
                            proxyDb.login_ = proxy.login_;
                            proxyDb.port = proxy.port;
                            proxyDb.type_id = proxy.type_id;
                            proxyDb.password_ = proxy.password_;
                            //ncSoftBase.Entry(proxy).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            ncSoftBase.proxys.Add(proxy);
                            //ncSoftBase.Entry(proxy).State = System.Data.Entity.EntityState.Added;
                        }
                    }
                    ncSoftBase.SaveChanges();
                }
            }
            
        }
    }
    
}
