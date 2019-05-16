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
    /// Interaction logic for AddEmails.xaml
    /// </summary>
    public partial class AddEmails : Window
    {
        public MyDataContext myDataContext = null;
        public AddEmails()
        {
            myDataContext = new MyDataContext();
            DataContext = myDataContext;
            InitializeComponent();
        }
        private void UploadEmailsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myDataContext.GetEmails();
            myDataContext.SaveEmails();
            DialogResult = true;
            Close();

        }
        public class MyDataContext:INotifyPropertyChanged
        {
            private List<email> listEmails;
            public List<email> ListEmails
            {
                get { return listEmails; }
                set { listEmails = value; OnPropertyChanged("ListEmails"); }
            }
            private string textEmails;
            public string TextEmails
            {
                get { return textEmails; }
                set { textEmails = value; OnPropertyChanged("EmailsText"); }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
            public List<email> GetEmails()
            {
                listEmails = new List<email>();
                if (textEmails == null)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    bool? rezDialog = openFileDialog.ShowDialog();
                    if (rezDialog == true)
                    {
                        textEmails = File.ReadAllText(openFileDialog.FileName);
                    }
                }
                if (textEmails is null)
                    return listEmails;
                var notEmptyStrings = textEmails.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in notEmptyStrings)
                {
                    var strings = s.Split(new char[] { ':',';' });
                    string email = strings[0];
                    string confirmEmail = "";
                    string phone = "";
                    string password = strings[1];
                    try
                    {
                        if (strings[2].IndexOf('@') != -1)
                        {
                            confirmEmail = strings[2];
                            phone = strings[3].Substring(0, 11);
                        }
                        else
                        {
                            if (strings[3].IndexOf('@') != -1)
                            {
                                confirmEmail = strings[3];
                                phone = strings[2].Substring(0, 11);
                            }
                        }
                        
                    }
                    catch { }
                    listEmails.Add(new email() { email1 = email, country_id = 1, confirm_email = confirmEmail, password_ = password, phone_confirm_email = phone  });
                }
                return listEmails;
            }
            public void SaveEmails()
            {
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    foreach (var email in listEmails)
                    {
                        
                        email.country_id = 1;
                        if (ncSoftBase.emails.Where(e => e.email1.Equals(email.email1)).Count() != 0)
                            email.id = ncSoftBase.emails.Where(e => e.email1.Equals(email.email1)).First().id;

                        //var v = ncSoftBase.proxys.Where(p => p.ip.Equals(proxy.ip)).Count();
                        if (ncSoftBase.emails.Where(e => e.email1.Equals(email.email1)).Count() != 0)
                        {
                            var emailDb = ncSoftBase.emails.Where(e => e.email1.Equals(email.email1)).First();
                            emailDb.email1 = email.email1;
                            emailDb.country_id = email.country_id;
                            emailDb.password_ = email.password_;
                            emailDb.confirm_email = email.confirm_email;
                            emailDb.phone_confirm_email = email.phone_confirm_email;
                            //ncSoftBase.Entry(proxy).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            ncSoftBase.emails.Add(email);
                            //ncSoftBase.Entry(proxy).State = System.Data.Entity.EntityState.Added;
                        }
                    }
                    ncSoftBase.SaveChanges();
                }
            }
            
        }
    }
}
