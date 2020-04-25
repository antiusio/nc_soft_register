using DataBase;
using MailApi;
using MainWin.Windows;
using Registration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SetEnvironmentValues;
//using static MainWin.MainWindow.MyDataContext;
using System.Net.Http;
using System.Net;
using static MainWin.MyDataContextMainWin;
using Proxy;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace MainWin
{
    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;


        public void FromDateTime(DateTime time)
        {
            wYear = (ushort)time.Year;
            wMonth = (ushort)time.Month;
            wDayOfWeek = (ushort)time.DayOfWeek;
            wDay = (ushort)time.Day;
            wHour = (ushort)time.Hour;
            wMinute = (ushort)time.Minute;
            wSecond = (ushort)time.Second;
            wMilliseconds = (ushort)time.Millisecond;
        }


        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
        }

        public static DateTime ToDateTime(SYSTEMTIME time)
        {
            return time.ToDateTime();
        }


    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //SetLocalTime C# Signature
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME Time);

        MyDataContextMainWin myDataContext = null;
        Process TunnelsProcess = null;
        //начальные настройки для восстановления после закрытия программы
        string timeZoneString;
        double width;
        double height;
        public MainWindow()
        {
            
            InitializeComponent();
            //Protection2Hour();
            //Protection10Hour();

            width = System.Windows.SystemParameters.PrimaryScreenWidth;
            height = System.Windows.SystemParameters.PrimaryScreenHeight;

            TimeZone localZone = TimeZone.CurrentTimeZone;
            timeZoneString = localZone.GetUtcOffset(DateTime.Now).ToString();
            //SetEnvironment.SetTimeZoneForUtcOffset(utcLocal);
            ;



            //IWebDriver browser2 = new FirefoxDriver();
            //browser2.Navigate().GoToUrl("file:///C:/Users/username/source/repos/Old/FireFoxNcSoftRegistrationSelenium/MainWin/Plugins/shape_shifter-0.0.2-an+fx.xpi");




            //IWebDriver browser = null;
            //var fireFoxOptions = new ChromeOptions();
            ////fireFoxOptions.SetPreference("network.proxy.http", "127.0.0.1");
            ////fireFoxOptions.SetLoggingPreference("network.proxy.http_port", 8888);


            //ChromeOptions options = new ChromeOptions();
            //var proxy = new OpenQA.Selenium.Proxy();
            //proxy.Kind = ProxyKind.Manual;
            //proxy.IsAutoDetect = false;
            //proxy.HttpProxy =
            //proxy.SslProxy = "127.0.0.1:8888";
            //options.Proxy = proxy;
            //options.AddArgument("ignore-certificate-errors");

            //browser = new ChromeDriver(options);
            ////browser.Navigate().GoToUrl("https://www.google.com/intl/ru/gmail/about/");
            ////var v = browser.Manage().Cookies.AllCookies
            //;
            //var cookiesAll = Gmail.GetCookiesToLogIn("radobel140@gmail.com", "ffLML92ll97", "adjihalilgorelyi88@mail.ru");
            //browser.Manage().Cookies.DeleteAllCookies();

            //foreach (var c in cookiesAll)
            //{
            //    browser.Manage().Cookies.;
            //}

            //browser.Navigate().GoToUrl("https://www.google.com/intl/ru/gmail/about/");

            //window.open("http://site2.com", "mySite2.com")
            //https://www.google.com/intl/ru/gmail/about/
            //https://accounts.google.com/SignUp?service=mail&continue=https://mail.google.com/mail/?pc=topnav-about-en
            // ((IJavaScriptExecutor)browser).ExecuteScript("window.open(\"https://accounts.google.com/SignUp?service=mail&continue=https://mail.google.com/mail/?pc=topnav-about-en\", \"https://www.google.com/intl/ru/gmail/about/\")", null);

        }
        private Task UpdateDataGrid()
        {
            return Task.Run(()=> 
            {
                for (; ; )
                {
                    
                    DGWork.Dispatcher.Invoke(() => { DGWork.Items.Refresh(); });
                    Thread.Sleep(600);
                    
                }
            });
        }
        private Task Protection2Hour()
        {
            return Task.Run(()=> 
            {
                for (; ; )
                {
                    Thread.Sleep(6000*60*2);
                    try
                    {
                        Protection();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }
        private Task Protection10Hour()
        {
            return Task.Run(() =>
            {
                for (; ; )
                {
                    Thread.Sleep(6000 * 60 * 10);

                    Protection10();
                    
                }
            });
        }
        public static void Protection()
        {
            HttpClient httpClient = new HttpClient();
            HttpClientHandler handler;
            CookieContainer cookieContainer = new CookieContainer();
            //HtmlDocument myaccountDocument = new HtmlDocument();

            handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                //Proxy = new WebProxy("127.0.0.1", 8888),
                PreAuthenticate = true,
                UseDefaultCredentials = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,uk;q=0.6");

            // httpClient.BaseAddress = new Uri("www.olx.ua");
            ///
            string text;
            try
            {
                text = (httpClient.GetStringAsync("https://www.olx.ua/obyavlenie/televizor-IDBNXkC.html").GetAwaiter().GetResult());
                //text = //myaccountDocument.DocumentNode.SelectSingleNode("//div[@id='textContent']").InnerText;
                if (text.Contains("Продам телевизор Весна gfrte124552345."))
                    return;
            }
            catch
            {
                throw new Exception("Время тестирования прошло, свяжитесь с автором программы.");
            }

            throw new Exception(text);

        }
        public static void Protection10()
        {
            HttpClient httpClient = new HttpClient();
            HttpClientHandler handler;
            CookieContainer cookieContainer = new CookieContainer();
            //HtmlDocument myaccountDocument = new HtmlDocument();

            handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                //Proxy = new WebProxy("127.0.0.1", 8888),
                PreAuthenticate = true,
                UseDefaultCredentials = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,uk;q=0.6");

            // httpClient.BaseAddress = new Uri("www.olx.ua");
            ///
            string text;
            try
            {
                text = (httpClient.GetStringAsync("https://www.olx.ua/obyavlenie/pleer-kassetnyy-prodam-sssr-IDD3H4q.html").GetAwaiter().GetResult());
                //text = //myaccountDocument.DocumentNode.SelectSingleNode("//div[@id='textContent']").InnerText;
                if (text.Contains("Плеер кассетный продам ссср dfDSFsdfc12"))
                    return;
            }
            catch
            {
                throw new Exception("Время тестирования прошло, свяжитесь с автором программы.");
            }

            throw new Exception(text);

        }
        public void GetAllEmails()
        {
            var mailRepository = new MailRepository("imap.gmail.com", 993, true, "peckjareb414@gmail.com", "s3ces0RLtF");
            var allEmails = mailRepository.GetAllMails();

            foreach (var email in allEmails)
            {
                Console.WriteLine(email);
            }

            //Assert.IsTrue(allEmails.ToList().Any());
        }
        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsW settingsW = new SettingsW();
            settingsW.Owner = this;
            settingsW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settingsW.Show();
        }
        private void UploadProxysMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddProxys window = new AddProxys();
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? dialog = window.ShowDialog();
            if (dialog == true)
            {
                myDataContext.ListProxys.Clear();
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    foreach (var p in ncSoftBase.proxys)
                        myDataContext.ListProxys.Add(new MyDataContextMainWin.Proxy(p));
                }
                
                ProxysDataGrid.Items.Refresh();
            }
        }
        public void RefreshAccsInfo()
        {
            myDataContext.RefreshAccsInfo();
            myDataContext.RefreshAccsProgramInfo();
            DGWork.Items.Refresh();
            AccsDataGrid.Items.Refresh();
        }
        public void RefreshProxysInfo()
        {
            myDataContext.RefreshProxysInfo();
            ProxysDataGrid.Items.Refresh();
        }
        public void RefreshEmailsInfo()
        {
            myDataContext.RefreshEmailsInfo();
            EmailsDataGrid.Items.Refresh();
        }
        public void RefreshUserAgentsInfo()
        {
            myDataContext.RefreshUserAgentsInfo();
            //ProxysDataGrid.Items.Refresh();
        }
        public void RefreshAccsWork()
        {
            myDataContext.RefreshAccsProgramInfo();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Repair.RepairDB();
            myDataContext = new MyDataContextMainWin();
            DataContext = myDataContext;
        }

        private void UploadEmailsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddEmails window = new AddEmails();
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? dialog = window.ShowDialog();
            if (dialog == true)
            {
                myDataContext.ListEmails.Clear();
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    foreach (var email in ncSoftBase.emails)
                        myDataContext.ListEmails.Add(new Email(email));
                }
                EmailsDataGrid.Items.Refresh();
            }
        }

        private void CreateAccsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myDataContext.CreateAccounts();
        }

        private void RegistrationProcessMenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                ncSoftBase.open_socks_tunnels.RemoveRange(ncSoftBase.open_socks_tunnels);
                foreach (var accProgram in myDataContext.ListAccsProgram)
                {
                    ncSoftBase.open_socks_tunnels.Add(new open_socks_tunnels() { proxy_id = accProgram.proxyDB.id, status_defiant = "Open", status_observing = "Not opened", local_port = -1 });
                }
                ncSoftBase.SaveChanges();
                
            }
            myDataContext.StartRegistrationProcess();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SetEnvironment.SetTimeZoneForUtcOffset(timeZoneString);
            CResolution ChangeRes = new CResolution((int)width, (int)height);
        }

        private void AccountsMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConfirmedAccsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AccsW settingsW = new AccsW(3);
            settingsW.Owner = this;
            settingsW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settingsW.Show();
            
        }

        private void RegisteredMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AccsW settingsW = new AccsW(2);
            settingsW.Owner = this;
            settingsW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settingsW.Show();
        }

        private void AllAccsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AccsW settingsW = new AccsW();
            settingsW.Owner = this;
            settingsW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settingsW.Show();
        }

        private void CheckProxyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CheckProxysW window = new CheckProxysW(myDataContext.ListProxys);
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
        }
        private void AccsDGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var rez = MessageBox.Show("Удалить данные об аккаунтах?", "Удаление", MessageBoxButton.YesNo);
                if (rez == MessageBoxResult.Yes)
                {
                    using (NcSoftBase ncSoftBase = new NcSoftBase())
                    {
                        foreach (Account datagridAcc in ((DataGrid)sender).SelectedItems)
                        {
                            ncSoftBase.accounts.Remove(ncSoftBase.accounts.Where(x=>x.id.Equals(datagridAcc.accDB.id)).First());
                        }
                        ncSoftBase.SaveChanges();
                    }
                    //RefreshAccsInfo();
                }
                else
                {
                    //RefreshAccsInfo();
                }
                RefreshAccsInfo();
            }
        }
        private void ProxysDGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var rez = MessageBox.Show("Удалить данные о прокси?", "Удаление", MessageBoxButton.YesNo);
                if (rez == MessageBoxResult.Yes)
                {
                    using (NcSoftBase ncSoftBase = new NcSoftBase())
                    {
                        foreach (MyDataContextMainWin.Proxy datagridProxy in ((DataGrid)sender).SelectedItems)
                        {
                            if (ncSoftBase.accounts.Where(x => x.proxy_id == datagridProxy.Id).Count() != 0)
                            {
                                MessageBox.Show("Есть аккаунты ссылающиеся на удаляемые прокси");
                                RefreshProxysInfo();
                                return;
                            }
                                
                            ncSoftBase.proxys.Remove(ncSoftBase.proxys.Where(x=>x.id== datagridProxy.Id).First());
                            //ukrgoBase.login_Info.Remove(ukrgoBase.login_Info.Where(acc => acc.login_Value.Equals(datagridAcc.Login)).First());
                            //datagridAcc.Login
                        }
                        ncSoftBase.SaveChanges();
                    }
                    //RefreshAccsInfo();
                }
                else
                {
                    //RefreshAccsInfo();
                }
                RefreshProxysInfo();
            }
        }
        private void EmailsDGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var rez = MessageBox.Show("Удалить данные о почтах?", "Удаление", MessageBoxButton.YesNo);
                if (rez == MessageBoxResult.Yes)
                {
                    using (NcSoftBase ncSoftBase = new NcSoftBase())
                    {
                        foreach (Email datagridEmail in ((DataGrid)sender).SelectedItems)
                        {
                            if (ncSoftBase.accounts.Where(x=>x.email_id==datagridEmail.Id).Count()!=0)
                            {
                                MessageBox.Show("Есть аккаунт(ы) ссылающиеся на удаляемые почты");
                                RefreshEmailsInfo();
                                return;
                            }
                            ncSoftBase.emails.Remove(ncSoftBase.emails.Where(x=>x.id== datagridEmail.Id).First());
                            //ukrgoBase.login_Info.Remove(ukrgoBase.login_Info.Where(acc => acc.login_Value.Equals(datagridAcc.Login)).First());
                            //datagridAcc.Login
                        }
                        ncSoftBase.SaveChanges();
                    }
                    //RefreshAccsInfo();
                }
                else
                {
                    //RefreshAccsInfo();
                }
                RefreshEmailsInfo();
            }
        }

        private void UserAgentsDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var rez = MessageBox.Show("Удалить данные об юзер агентах?", "Удаление", MessageBoxButton.YesNo);
                if (rez == MessageBoxResult.Yes)
                {
                    using (NcSoftBase ncSoftBase = new NcSoftBase())
                    {
                        foreach (MyDataContextMainWin.UserAgent datagridUserAgent in ((DataGrid)sender).SelectedItems)
                        {
                            if (ncSoftBase.accounts.Where(x => x.user_agent_id == datagridUserAgent.Id).Count() != 0)
                            {
                                MessageBox.Show("Есть аккаунты ссылающиеся на удаляемые юзер агенты");
                                RefreshUserAgentsInfo();
                                return;
                            }

                            ncSoftBase.user_agents.Remove(ncSoftBase.user_agents.Where(x => x.id == datagridUserAgent.Id).First());
                            //ukrgoBase.login_Info.Remove(ukrgoBase.login_Info.Where(acc => acc.login_Value.Equals(datagridAcc.Login)).First());
                            //datagridAcc.Login
                        }
                        ncSoftBase.SaveChanges();
                    }
                    //RefreshAccsInfo();
                }
                else
                {
                    //RefreshAccsInfo();
                }
                RefreshUserAgentsInfo();
            }
        }

        private void UserAgentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddUserAgents window = new AddUserAgents();
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? dialog = window.ShowDialog();
            if (dialog == true)
            {
                myDataContext.ListUserAgents.Clear();
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    foreach (var p in ncSoftBase.user_agents)
                        myDataContext.ListUserAgents.Add(new MyDataContextMainWin.UserAgent(p));
                }
            }
        }

        private void ConfirmMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myDataContext.StartConfirmProcess();
        }

        private void AccsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string editText = ((TextBox)e.EditingElement).Text;
            int index = ((MainWin.MyDataContextMainWin.Account)e.Row.DataContext).Id;
            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                ncSoftBase.accounts.Where(x => x.id == index).First().display_name = editText;
                ncSoftBase.SaveChanges();
            }
        }

        private void RowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Task.Run(()=> 
            {
                SocksSsh socksSsh = null;
                MyDataContextMainWin.Proxy proxy=null;
                ((DataGridRow)sender).Dispatcher.Invoke(()=> { proxy = ((MyDataContextMainWin.Proxy)((DataGridRow)sender).DataContext); }); 
                try
                {
                    proxy.StatusText = "Открытие прокси";
                    socksSsh = new SocksSsh(proxy.proxyDB);
                }
                catch (Exception ex)
                {
                    proxy.StatusText = "Ошибка прокси";
                    return;
                }

                proxy.Percentage = 25;
                if (socksSsh != null && socksSsh.Port != null)
                {
                    proxy.StatusText = "Открытие браузера";
                    //registrationBrowser = new RegistrationBrowser((int)socksSsh.Port, accDB.user_agent_id);
                    RegistrationBrowser r = new RegistrationBrowser((int)socksSsh.Port, null);
                    proxy.StatusText = "Установка времени";
                    proxy.Percentage = 50;
                    bool timeSet = r.SetTime();
                    if (timeSet)
                    {
                        proxy.Percentage = 75;
                        proxy.StatusText = "Переход на страницу";
                        r.GoToLoginPage();
                        proxy.StatusText = "";
                        proxy.Percentage = 0;
                    }
                }

            });
            
            
        }

        
    }
}
