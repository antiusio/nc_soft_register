using DataBase;
using HtmlAgilityPack;
using MailApi;
using Proxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Registration
{
    
    public class Acc:INotifyPropertyChanged
    {
        public static string[] Names=null;
        public static int indexName;
        Tunnels t = null;
        public proxy proxyDB=null;
        private SocksSsh socksSsh=null;
        private RegistrationBrowser registrationBrowser=null;
        private account accDB = null;
        private email emailDB = null;
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
        public string ChangeName()
        {
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                try
                {
                    ncSoftBase.accounts.Where(x => x.id == accDB.id).First().display_name = Names[indexName];
                    indexName++;
                    ncSoftBase.SaveChanges();
                }
                catch { }
                return Names[(indexName-1)];
            }
        }
        public Acc(account acc)
        {
            if (Names == null)
            {
                Names = GetUserNames();
                indexName = 0;
            }
            StatusText = "Сформированы данные";
            IdUserAgent = acc.user_agent_id;
            if (acc.status_id==3)
                StatusText = "Подтвержденный";
            if (acc.status_id == 2)
                StatusText = "Зарегистрированный";
            this.accDB = acc;
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                proxyDB=ncSoftBase.proxys.Where(p=>p.id.Equals(acc.proxy_id)).First();
                emailDB = ncSoftBase.emails.Where(em => em.id.Equals(acc.email_id)).First();
                Email = emailDB.email1;
                IpProxy = proxyDB.ip;
                Id = emailDB.id;
                RegStatus = ncSoftBase.statuses_registration.Where(x=>x.id==acc.status_id).First().text_status;
                //RegStatus = acc.statuses_registration.text_status;
            }
        }
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        private int? idUserAgent;
        public int? IdUserAgent
        {
            get { return idUserAgent; }
            set { idUserAgent = value; }
        }

        private string regStatus;
        public string RegStatus
        {
            get { return regStatus; }
            set { regStatus = value; OnPropertyChanged("RegStatus"); }
        }
        private string email;
        public string Email { get { return email; } set {email = value; OnPropertyChanged("Email"); } }
        private string ipProxy;
        public string IpProxy { get { return ipProxy; } set { ipProxy = value; OnPropertyChanged("IpProxy"); } }


        private string statusText;
        public string StatusText { get { return statusText; } set { statusText = value; OnPropertyChanged("StatusText"); } }
        private int percentage;
        public int Percentage { get { return percentage; } set { percentage = value; OnPropertyChanged("Percentage"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private void CloseProxyAndBrowser()
        {
            //using (NcSoftBase ncSoftBase = new NcSoftBase())
            //{
            //    var socks = ncSoftBase.open_socks_tunnels.Where(s => s.proxy_id == proxyDB.id).First();
            //    socks.status_defiant = "Close";
            //    ncSoftBase.SaveChanges();
            //}
            try
            {
                if (registrationBrowser != null)
                    registrationBrowser.Close();
            }
            catch { }
        }

        public void SetSystemTimeZone()
        {
            
        }
        public Task<bool> Registration()
        {
            return Task<bool>.Run(()=>
            {
                StatusText = "Начало регистрации";
                Percentage = 5;
                StatusText = "Настройка прокси";

                try
                {
                    socksSsh = new SocksSsh(proxyDB);
                }
                catch (Exception ex)
                {
                    StatusText = "Ошибка прокси";
                    return false;
                }

                Percentage = 25;
                if (socksSsh != null && socksSsh.Port != null)
                {
                    StatusText = "Открытие браузера";
                    registrationBrowser = new RegistrationBrowser((int)socksSsh.Port, accDB.user_agent_id);
                }
                //int port = -1;
                //for (; ; )
                //    using (NcSoftBase ncSoftBase = new NcSoftBase())
                //    {
                //        var socks = ncSoftBase.open_socks_tunnels.Where(s => s.proxy_id == proxyDB.id).First();
                //        if (socks.local_port != -1)
                //        {
                //            port = (int)socks.local_port;
                //            break;
                //        }
                //        if (socks.status_observing.Equals("Not opens"))
                //        {
                //            StatusText = "Порт не открыт";
                //            Percentage = 0;
                //            break;
                //        }
                //    }
                //if(port!=-1)
                //    registrationBrowser = new RegistrationBrowser(port);
                if (registrationBrowser == null)
                {
                    StatusText = "Прокси не открыт";
                    Percentage = 0;
                    CloseProxyAndBrowser();
                    return false;
                }
                else
                {
                    StatusText = "Браузер открыт";
                    Percentage = 35;
                }
                //StatusText = "Ожидание прокси и открытие браузера";
                StatusText = "Настройка параметров внешней среды";
                bool f = registrationBrowser.SetTime();
                if (!f)
                {
                    StatusText = "Не удалось установить параметры внешней среды";
                    Percentage = 0;
                    CloseProxyAndBrowser();
                    return false;

                }
                bool fromBase = false;
                bool registered = false;
                if (RegStatus.Equals("created"))
                    registered = registrationBrowser.StartRegistration2(accDB,this);
                if (RegStatus.Equals("registered"))
                {
                    registered = true;
                    fromBase = true;
                }
                if (!registered)
                {
                    //StatusText = "Аккаунт не получилось зарегистрировать";
                    Percentage = 0;
                    CloseProxyAndBrowser();
                    return false;
                }
                if(!fromBase)
                    using (NcSoftBase ncSoftBase = new NcSoftBase())
                    {
                        var acc=ncSoftBase.accounts.Where(ac => ac.id == accDB.id).First();
                        acc.status_id = 2;
                        acc.count_try = registrationBrowser.indexTry;
                        ncSoftBase.SaveChanges();
                        RegStatus = ncSoftBase.statuses_registration.Where(x => x.id == acc.status_id).First().text_status;
                    }
                for(; ; )
                {
                    break;
                }
                //StatusText = "Аккаунт зарегистрирован, начинаем подтверждение";
                C:
                bool confirmed = false;
                StatusText = "Получение ссылки подтверждения из почты";
                for (int i = 0; i < 1; i++)
                {
                    if (confirmed)
                        break;
                    string link = Gmail.GetConfirmLink2(emailDB.email1, emailDB.password_, emailDB.confirm_email);
                    //radobel140@gmail.com;ffLML92ll97;+79172615513;adjihalilgorelyi88@mail.ru
                    //Gmail.GetEmailsString(emailDB.email1,emailDB.password_,emailDB.confirm_email);
                    //if (link.Equals(""))
                    //{
                    //    if (!getRepeatEmail())
                    //        break;
                    //    Thread.Sleep(3000);
                    //    continue;
                    //}

                    if (!link.Equals(""))
                    {
                        StatusText = "Подтверждение по ссылке";
                        confirmed = registrationBrowser.ConfirmWithLink(link);
                        //StatusText = "Мои поздравления";
                        if (!confirmed)
                        {
                            //StatusText = "Минутку";
                            //Thread.Sleep(6000);
                            //goto C;
                        }
                    }
                    //else
                    //{
                    //    StatusText = "Переотправка письма";
                    //    bool rez = registrationBrowser.ResentVerificationEmail();
                    //    if (!rez)
                    //        StatusText = "Переотправка не удалась";
                    //    else
                    //        StatusText = "Переотправка сработала";
                    //    Thread.Sleep(1000);
                    //    //link = Gmail.GetConfirmLink2(emailDB.email1, emailDB.password_, emailDB.confirm_email);
                    //    ;
                    //}
                }
                if (confirmed)
                {
                    using (NcSoftBase ncSoftBase = new NcSoftBase())
                    {
                        var acc = ncSoftBase.accounts.Where(ac => ac.id == accDB.id).First();
                        acc.status_id = 3;
                        acc.date_confirmed = DateTime.Now;
                        RegStatus = ncSoftBase.statuses_registration.Where(x=>x.id== acc.status_id).First().text_status;
                        acc.count_try = registrationBrowser.indexTry;
                        ncSoftBase.SaveChanges();
                    }
                }
                else
                {
                    StatusText = "Аккаунт зарегистрирован, но не подтвержден, произошла ошибка, или не пришло письмо";
                    Percentage = 50;
                    CloseProxyAndBrowser();
                    return false;
                }
                StatusText = "Аккаунт зарегистрирован и подтвержден из почты";
                
                Percentage = 100;
                CloseProxyAndBrowser();
                return true;
            });
        }
        private bool getRepeatEmail()
        {
            return false;
        }
    }
}
