using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for SettingsW.xaml
    /// </summary>
    public partial class SettingsW : Window
    {
        MyDataContext myDataContext = null;
        public SettingsW()
        {
            InitializeComponent();
            myDataContext = new MyDataContext();
            DataContext = myDataContext;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            myDataContext.SaveChanges();
        }
    }
    public class MyDataContext : INotifyPropertyChanged
    {
        public MyDataContext()
        {
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                setting s = ncSoftBase.settings.First<setting>();
                captchaApiKey = s.captcha_api_key;
                countThreads = s.count_threads;
            }
        }
        private string captchaApiKey;
        public string CaptchaApiKey
        {
            get { return captchaApiKey; }
            set { captchaApiKey = value; OnPropertyChanged("ApiKey"); }
        }
        private int countThreads;
        public int CountThreads
        {
            get { return countThreads; }
            set { countThreads = value; OnPropertyChanged("CountThreads"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public void SaveChanges()
        {
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                setting s = ncSoftBase.settings.First();
                s.captcha_api_key = captchaApiKey;
                s.count_threads = countThreads;
                ncSoftBase.settings.Add(s);
                ncSoftBase.Entry(s).State = System.Data.Entity.EntityState.Modified;
                ncSoftBase.SaveChanges();
            }
        }
    }
}
