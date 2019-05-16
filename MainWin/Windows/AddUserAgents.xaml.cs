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
    /// Interaction logic for AddUserAgents.xaml
    /// </summary>
    public partial class AddUserAgents : Window
    {
        MyDataContext myDataContext = null;
        public AddUserAgents()
        {
            InitializeComponent();
            myDataContext = new MyDataContext();
            DataContext = myDataContext;
        }

        private void UploadUserAgentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myDataContext.GetUserAgents();
            myDataContext.SaveUserAgents();
            DialogResult = true;
            Close();
        }
        public class MyDataContext : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop="")
            {
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prop));
            }
            private string textUserAgents;
            public string TextUserAgents
            {
                get { return textUserAgents; }
                set { textUserAgents = value; OnPropertyChanged("TextUserAgents"); }
            }
            public List<user_agents> listUserAgents = null;
            public List<user_agents> GetUserAgents()
            {
                listUserAgents = new List<user_agents>();
                if (textUserAgents == null)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    bool? rezDialog = openFileDialog.ShowDialog();
                    if (rezDialog == true)
                    {
                        textUserAgents = File.ReadAllText(openFileDialog.FileName);
                    }
                }
                if (textUserAgents is null)
                    return listUserAgents;
                var notEmptyStrings = textUserAgents.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in notEmptyStrings)
                {
                    
                    listUserAgents.Add(new user_agents() { value =s });
                }
                return listUserAgents;
            }
            public void SaveUserAgents()
            {
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    foreach (var agent in listUserAgents)
                    {
                        
                        if (ncSoftBase.user_agents.Where(p => p.value.Equals(agent.value)).Count() != 0)
                            agent.id = ncSoftBase.user_agents.Where(p => p.value.Equals(agent.value)).First().id;

                        //var v = ncSoftBase.proxys.Where(p => p.ip.Equals(proxy.ip)).Count();
                        if (ncSoftBase.user_agents.Where(p => p.value.Equals(agent.value)).Count() != 0)
                        {
                            //ncSoftBase.Entry(proxy).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            ncSoftBase.user_agents.Add(agent);
                            //ncSoftBase.Entry(proxy).State = System.Data.Entity.EntityState.Added;
                        }
                    }
                    ncSoftBase.SaveChanges();
                }
            }
        }
    }
}
