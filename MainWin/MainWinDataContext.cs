using DataBase;
using HtmlAgilityPack;
using Registration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainWin
{
    public class MyDataContextMainWin : INotifyPropertyChanged
    {
        private ObservableCollection<Acc> listAccsProgram;
        public ObservableCollection<Acc> ListAccsProgram
        {
            get { return listAccsProgram; }
            set { listAccsProgram = value; OnPropertyChanged("ListAccsProgram"); }
        }
        private string[] Names = null;

        private ObservableCollection<Proxy> listProxys;
        public ObservableCollection<Proxy> ListProxys
        {
            get { return listProxys; }
            set { listProxys = value; OnPropertyChanged("ListProxys"); }
        }
        public class Proxy : INotifyPropertyChanged
        {
            public proxy proxyDB;
            public Proxy(proxy proxyDB)
            {
                this.proxyDB = proxyDB;
                Id = proxyDB.id;
                Ip = proxyDB.ip;
                Port = proxyDB.port;
                Login = proxyDB.login_;
                Password = proxyDB.password_;
            }
            private int id;
            public int Id
            {
                get { return id; }
                set { id = value; OnPropertyChanged("Id"); }
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
            private string login;
            public string Login
            {
                get { return login; }
                set { login = value; OnPropertyChanged("Login"); }
            }
            private string password;
            public string Password
            {
                get { return password; }
                set { password = value; OnPropertyChanged("Password"); }
            }
            private string status;
            public string Status
            {
                get { return status; }
                set { status = value; OnPropertyChanged("Status"); }
            }
            private string statusText;
            public string StatusText
            {
                get { return statusText; }
                set { statusText = value; OnPropertyChanged("StatusText"); }
            }
            private int percentage;
            public int Percentage
            {
                get { return percentage; }
                set { percentage = value; OnPropertyChanged("Percentage"); }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop="")
            {
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prop));
            }
        }
        public class UserAgent : INotifyPropertyChanged
        {
            public user_agents userAgent;
            public UserAgent(user_agents userAgent)
            {
                this.userAgent = userAgent;
                Id = userAgent.id;
                Agent = userAgent.value;
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
            private int id;
            public int Id
            {
                get { return id; }
                set { id = value; OnPropertyChanged("Id"); }
            }
            private string agent;
            public string Agent
            {
                get { return agent; }
                set { agent = value; OnPropertyChanged("Agent"); }
            }

        }
        private ObservableCollection<UserAgent> listUserAgents;
        public ObservableCollection<UserAgent> ListUserAgents
        {
            get { return listUserAgents; }
            set { listUserAgents = value; OnPropertyChanged("ListUserAgents"); }
        }
        private ObservableCollection<Email> listEmails;
        public ObservableCollection<Email> ListEmails
        {
            get { return listEmails; }
            set { listEmails = value; OnPropertyChanged("ListEmails"); }
        }
        public class Email : INotifyPropertyChanged
        {

            private email emailDB;
            public Email(email emailDB)
            {
                this.emailDB = emailDB;
                Id = emailDB.id;
                Email_ = emailDB.email1;
                Password = emailDB.password_;
                ConfirmEmail = emailDB.confirm_email;
            }
            private int id;
            public int Id
            {
                get { return id; }
                set { id = value; OnPropertyChanged("Id"); }
            }
            private string email_;
            public string Email_
            {
                get { return email_; }
                set { email_ = value; OnPropertyChanged("Email"); }
            }
            private string password;
            public string Password
            {
                get { return password; }
                set { password = value; OnPropertyChanged("Password"); }
            }
            private string confirmEmail;
            public string ConfirmEmail
            {
                get { return confirmEmail; }
                set { confirmEmail = value; OnPropertyChanged("ConfirmEmail"); }
            }
            private string status;
            public string Status
            {
                get { return status; }
                set { status = value; OnPropertyChanged("Status"); }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
        private ObservableCollection<Account> listAccounts;
        public ObservableCollection<Account> ListAccounts
        {
            get { return listAccounts; }
            set { listAccounts = value; OnPropertyChanged("ListAccounts"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public class Account : INotifyPropertyChanged
        {
            private int id;
            public int Id
            {
                get { return id; }
                set { id = value; OnPropertyChanged("Id"); }
            }
            private string email;
            public string Email
            {
                get { return email; }
                set { email = value; OnPropertyChanged("Email"); }
            }
            private string password;
            public string Password
            {
                get { return password; }
                set { password = value; OnPropertyChanged("Password"); }
            }
            private DateTime dateCreated;
            public DateTime DateCreated
            {
                get { return dateCreated; }
                set { dateCreated = value; OnPropertyChanged("DateCreated"); }
            }
            private string displayName;
            public string DisplayName
            {
                get { return displayName; }
                set { displayName = value; OnPropertyChanged("DisplayName"); }
            }
            private DateTime? dateRegistered;
            public DateTime? DateRegistered
            {
                get { return dateRegistered; }
                set { dateRegistered = value; OnPropertyChanged("DateRegistered"); }
            }
            private DateTime? dateConfirmed;
            public DateTime? DateConfirmed
            {
                get { return dateConfirmed; }
                set { dateConfirmed = value; OnPropertyChanged("DateConfirmed"); }
            }
            private string status;
            public string Status
            {
                get { return status; }
                set { status = value; OnPropertyChanged("Status");}
            }
            private int? idUserAgent;
            public int? IdUserAgent
            {
                get { return idUserAgent; }
                set { idUserAgent = value; }
            }
            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
            public account accDB = null;
            public Account(account acc)
            {
                this.accDB = acc;
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    Email = ncSoftBase.emails.Where(x => x.id.Equals(acc.email_id)).First().email1;
                    Status = ncSoftBase.statuses_registration.Where(x => x.id == acc.status_id).First().text_status;
                    
                }
                IdUserAgent = acc.user_agent_id;
                Id = acc.id;
                Password = acc.password_;
                DateCreated = acc.date_created;
                DateRegistered = acc.date_registered;
                DateConfirmed = acc.date_confirmed;
                DisplayName = acc.display_name;

            }
        }
        public MyDataContextMainWin()
        {
            ListProxys = new ObservableCollection<Proxy>();
            ListEmails = new ObservableCollection<Email>();
            ListAccounts = new ObservableCollection<Account>();
            ListAccsProgram = new ObservableCollection<Acc>();
            ListUserAgents = new ObservableCollection<UserAgent>();
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                //var accs = ncSoftBase.accounts.Where(acc => acc.status_id != 3);
                //int n = accs.Count();
                //var ac = accs.ElementAt(1);
                //if (accs.Count()!=0)
                //    ListAccsProgram.Add(new Acc(accs.ElementAt(0)));
                foreach (var p in ncSoftBase.proxys)
                {
                    ListProxys.Add(new Proxy(p));
                }
                foreach (var email in ncSoftBase.emails)
                {
                    ListEmails.Add(new Email(email));
                }
                foreach (var acc in ncSoftBase.accounts)
                {
                    if (acc.status_id != 3)
                        ListAccsProgram.Add(new Acc(acc));
                    ListAccounts.Add(new Account(acc));
                    //var acc2 = listAccounts[listAccounts.Count - 1]
                    string status = ncSoftBase.statuses_registration.Where(x => x.id == acc.status_id).First().text_status;
                    ListProxys.Where(x => x.Id.Equals(acc.proxy_id)).First().Status = status;
                    ListEmails.Where(x=>x.Id.Equals(acc.email_id)).First().Status = status;
                    //if(listAccounts[listAccounts.Count-1].Status)
                }
                foreach( var userAgent in ncSoftBase.user_agents)
                {
                    ListUserAgents.Add(new UserAgent(userAgent));
                }
                
            }
            //Random r = new Random(DateTime.Now.Millisecond);
            Names = GetUserNames();
            //listProxys.Add(new proxy() { ip = "sdfdfs" });
        }
        public static string[] GetUserNames()
        {
            HttpClient client = new HttpClient();
            string htmlNames = client.GetStringAsync("https://jimpix.co.uk/words/random-username-list.asp").GetAwaiter().GetResult();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlNames);
            //var nodeUl=doc.DocumentNode.SelectSingleNode("//ul[@class='list-unstyled']");
            var nodes = doc.DocumentNode.SelectNodes("//ul[@class='list-unstyled']/li");
            var Names = new string[200];
            for (int i = 0; i < Names.Length; i++)
            {
                Names[i] = nodes[i].InnerText.Trim();
            }

            for (int i = 0; i < Names.Count(); i++)
            {
                if (Names[i].Count() > 16)
                    Names[i] = Names[i].Remove(15);
            }
            return Names;
        }
        public ObservableCollection<Account> CreateAccounts()
        {
            List<account> listCreatedAccs = new List<account>();
            int indexUserAgent = 0;
            if (listUserAgents.Count == 0)
            {
                MessageBox.Show("Юзер агентов нету");
                return new ObservableCollection<Account>();
            }
            Random r = new Random(DateTime.Now.Millisecond);
            if (listEmails.Count != 0 && listProxys.Count != 0 && (listEmails.Count == listProxys.Count || listEmails.Count < listProxys.Count))
            {
                listAccounts.Clear();
                for (int i = 0; i < listEmails.Count; i++)
                {
                    var dateBirth = DateTime.Now;
                    dateBirth = dateBirth.AddYears(-r.Next(18, 50));
                    dateBirth = dateBirth.AddMonths(r.Next(0, 12));
                    dateBirth = dateBirth.AddDays(r.Next(5, 20));

                    listAccounts.Add(new Account(new account()
                    {
                        date_created = DateTime.Now,
                        date_of_birth = dateBirth,
                        email_id = listEmails[i].Id,
                        proxy_id = listProxys[i].Id,
                        password_ = listEmails[i].Password + r.Next(0, 9) + listEmails[i].Password[0].ToString().ToUpper(),
                        status_id = 1,
                        display_name = Names[r.Next(0, Names.Length)],
                        user_agent_id = listUserAgents[indexUserAgent].Id
                    }));
                    indexUserAgent++;
                    if (indexUserAgent >= listUserAgents.Count)
                        indexUserAgent = 0;
                }
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    //account a = ncSoftBase.accounts.First();
                    //int n =ncSoftBase.statuses_registration.Count();
                    foreach (Account acc in listAccounts)
                    {
                        if (ncSoftBase.accounts.Where(ac => ac.email_id.Equals(acc.accDB.email_id)).Count() == 0)
                        {
                            ncSoftBase.accounts.Add(acc.accDB);
                            //ncSoftBase.SaveChanges();
                            listCreatedAccs.Add(acc.accDB);
                        }
                        else
                        {
                            var acDB = ncSoftBase.accounts.Where(ac => ac.email_id.Equals(acc.accDB.email_id)).First();
                            acDB.proxy_id = acc.accDB.proxy_id;
                            listCreatedAccs.Add(acDB);
                        }
                    }

                    ncSoftBase.SaveChanges();
                    ListAccounts.Clear();
                    foreach (account acc in listCreatedAccs)
                    {
                        if (acc.status_id == 3)
                            continue;
                        ListAccounts.Add(new Account(ncSoftBase.accounts.Where(ac => ac.email_id.Equals(acc.email_id)).First()));
                    }
                }
            }
            else
                MessageBox.Show("Почт больше чем проксей");
            ListAccsProgram.Clear();
            foreach (var acc in ListAccounts)
                ListAccsProgram.Add(new Acc(acc.accDB));
            return ListAccounts;
        }
        public void RefreshAccsInfo()
        {
            ListAccounts.Clear();
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                foreach (account acc in ncSoftBase.accounts)
                {
                    ListAccounts.Add(new Account(acc));
                }
            }

        }
        public void RefreshAccsProgramInfo()
        {
            listAccsProgram.Clear();
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                foreach (account acc in ncSoftBase.accounts)
                {
                    listAccsProgram.Add(new Acc(acc));
                }
            }

        }
        public void RefreshProxysInfo()
        {
            ListProxys.Clear();
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                foreach (proxy p in ncSoftBase.proxys)
                {
                    ListProxys.Add(new Proxy(p));
                }
            }

        }
        public void RefreshUserAgentsInfo()
        {
            ListProxys.Clear();
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                foreach (user_agents p in ncSoftBase.user_agents)
                {
                    ListUserAgents.Add(new UserAgent(p));
                }
            }

        }
        public void RefreshEmailsInfo()
        {
            ListEmails.Clear();
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                foreach (email em in ncSoftBase.emails)
                {
                    ListEmails.Add(new Email(em));
                }
            }

        }
        public void StartRegistrationProcess()
        {
            Task.Run(() =>
            {
                int countThreads = 0;
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    countThreads = ncSoftBase.settings.First().count_threads;
                }
                for (int i = 0; i < ListAccsProgram.Count; i++)
                {
                    Task<bool>[] tasks = null;
                    if (i + countThreads <= listAccsProgram.Count)
                        tasks = new Task<bool>[countThreads];
                    for (int j = 0; j + i < listAccsProgram.Count && j + i < i + countThreads; j++)
                        tasks[j] = listAccsProgram[i + j].Registration();
                    Task.WaitAll(tasks);
                    for(int j = 0; j + i < listAccsProgram.Count && j + i < i + countThreads; j++)
                    {
                        ListAccounts[i+j].Status = listAccsProgram[i + j].RegStatus;
                        ListProxys.Where(x=>x.Ip.Equals(listAccsProgram[i + j].IpProxy)).First().Status = listAccsProgram[i + j].RegStatus;
                    }
                }
                //подтверждение, если есть не подтвержденные аккаунты
                StartConfirmProcess();



            });
            
        }
        public void StartConfirmProcess()
        {
            Task.Run(() =>
            {
                int countThreads = 0;
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    countThreads = ncSoftBase.settings.First().count_threads;
                }
                var listRegistered = ListAccsProgram.Where(x => x.RegStatus.Equals("registered"));
                if (!(listRegistered != null && listRegistered.Count() > 0))
                    return;
                var listRegisteredProgram = listRegistered.ToList();
                for (int i = 0; i < listRegistered.Count(); i++)
                {
                    Task<bool>[] tasks = null;
                    if (i + countThreads <= listRegisteredProgram.Count())
                        tasks = new Task<bool>[countThreads];
                    for (int j = 0; j + i < listRegisteredProgram.Count() && j + i < i + countThreads; j++)
                        tasks[j] = listRegisteredProgram[i + j].Registration();
                    Task.WaitAll(tasks);
                    for (int j = 0; j + i < listRegisteredProgram.Count() && j + i < i + countThreads; j++)
                    {
                        ListAccounts[i + j].Status = listRegisteredProgram[i + j].RegStatus;
                        ListProxys.Where(x => x.Ip.Equals(listRegisteredProgram[i + j].IpProxy)).First().Status = listRegisteredProgram[i + j].RegStatus;
                    }
                }
            });
        }
    }
}
