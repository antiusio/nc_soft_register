using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Net.Http;
using DataBase;
using SetEnvironmentValues;
using System.Windows.Forms;


namespace Registration
{
    public class RegistrationBrowser
    {


        //string url = "http://f.vision/";
        //https://account.ncsoft.com/signup/index?serviceCode=13#
        string url = "https://account.ncsoft.com/signup/index?serviceCode=13#";
        IWebDriver browser = null;
        public RegistrationBrowser(int socksPort, int? userAgentId)
        {
            //CResolution ChangeRes = new CResolution(1280,768);
            //Environment.SetEnvironmentVariable();
            FirefoxProfile firefoxProfile = new FirefoxProfile();
            Random r = new Random(DateTime.Now.Millisecond);
            //media.video_stats.enabled
            firefoxProfile.SetPreference("media.video_stats.enabled", false);
            //privacy.resistFingerprinting.reduceTimerPrecision.microseconds
            firefoxProfile.SetPreference("privacy.resistFingerprinting.reduceTimerPrecision.microseconds", r.Next(20,100));
            //webgl.disabled
            firefoxProfile.SetPreference("webgl.disabled", true);
            //media.navigator.enabled
            firefoxProfile.SetPreference("media.navigator.enabled", false);
            //privacy.resistFingerprinting
            //firefoxProfile.SetPreference("privacy.resistFingerprinting", true);
            //reader.font_size
            firefoxProfile.SetPreference("reader.font_size", r.Next(1, 20));
            //canvas.path.enabled
            firefoxProfile.SetPreference("canvas.path.enabled", false);
            //media.navigator.audio.fake_frequency
            firefoxProfile.SetPreference("media.navigator.audio.fake_frequency", r.Next(800, 1200));
            //media.recorder.audio_node.enabled
            firefoxProfile.SetPreference("media.recorder.audio_node.enabled", false);
            //dom.webaudio.enabled
            firefoxProfile.SetPreference("dom.webaudio.enabled", false);
            //gfx.downloadable_fonts.enabled
            firefoxProfile.SetPreference("gfx.downloadable_fonts.enabled", false);

            
            //string pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\shape_shifter-0.0.2-an+fx.xpi";
            //firefoxProfile.AddExtension(pathToExtension);
            //firefoxProfile.SetPreference("extensions.shape_shifter.currentVersion", "0.0.2");
            

            //pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\firebug-1.8.1.xpi";
            //firefoxProfile.AddExtension(pathToExtension);
            //firefoxProfile.SetPreference("extensions.firebug.currentVersion", "1.8.1");
            //var allProfiles = new FirefoxProfileManager();
            //
            //set the webdriver_assume_untrusted_issuer to false
            firefoxProfile.SetPreference("webdriver_assume_untrusted_issuer", false);


            //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (X11; Linux x86_64; rv:52.0) Gecko/20100101 Firefox/52.0");
            //Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:61.0) Gecko/20100101 Firefox/61.0
            //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:61.0) Gecko/20100101 Firefox/61.0");
            firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (X11; Linux x86_64; rv:52.0) Gecko/20100101 Firefox/52.0");
            try
            {
                using (NcSoftBase ncSoftBase = new NcSoftBase())
                {
                    string s = ncSoftBase.user_agents.Where(x => x.id == userAgentId).First().value;
                    firefoxProfile.SetPreference("general.useragent.override", s);
                }
            }
            catch{ }
            

            //if (!allProfiles.ExistingProfiles.Contains("SeleniumUser"))
            //{
            //   throw new Exception("SeleniumUser firefox profile does not exist, please create it first.");
            //}
            //var profile = allProfiles.GetProfile("SeleniumUser");
            //string s = Directory.GetCurrentDirectory();
            //File.Open(@"Plugins\shape_shifter-0.0.2-an+fx.xpi",FileMode.Open);
            var fireFoxOptions = new FirefoxOptions();
            fireFoxOptions.Profile = firefoxProfile;
            fireFoxOptions.SetPreference("network.proxy.type", 1);
            fireFoxOptions.SetPreference("network.proxy.socks", "127.0.0.1");


            fireFoxOptions.SetPreference("network.proxy.socks_port", socksPort);    
            
            
            //fireFoxOptions.SetPreference("network.proxy.http", "7951");
            //fireFoxOptions.SetPreference("network.proxy.socks_port", 40348);
            fireFoxOptions.SetPreference("media.peerconnection.enabled", false);
            //network.proxy.socks_remote_dns

            //;
            fireFoxOptions.SetPreference("network.proxy.socks_remote_dns", true);
            //;

            //fireFoxOptions.SetLoggingPreference
            //Mozilla/5.0 (Linux; U; Android 4.4 Andro-id Build/KRT16S; X11; FxOS armv7I rv:29.0) MyWebkit/537.51.1 (KHTML, like Gecko) Gecko/29.0 Firefox/29.0
            

            //profile.update_preferences()

            //# You would also like to block flash 
            //           profile.set_preference('dom.ipc.plugins.enabled.libflashplayer.so', False)
            //profile.set_preference("media.peerconnection.enabled", False)

            browser = new FirefoxDriver(fireFoxOptions);
            //browser.Manage().Window.Size = new Size(1440, 900);

            

        }
        public void Test()
        {
            browser.Navigate().GoToUrl("http://f.vision/");
        }
        public void GoToLoginPage()
        {
            browser.Navigate().GoToUrl("https://login.ncsoft.com/login?service_code=13");
        }
        public bool SetTime()
        {
            IWebElement webElement=null;
            for (int i=0;i<=5;i++)
                try
                {
                    try
                    {
                        browser.Navigate().GoToUrl("http://f.vision/");
                        Thread.Sleep(3000);
                    }
                    
                    catch (Exception ex)
                    {
                        if (ex.Message != null && ex.Message.IndexOf("proxyConnectFailure") != -1)
                            return false;
                        if (ex.InnerException!=null && ex.InnerException.Message.IndexOf("The operation has timed out") != -1)
                        {
                            return false;
                        }
                    }
                    webElement = browser.FindElement(By.Id("q-tz-ip-test"));
                    string textTime = webElement.Text;
                    SetEnvironment.SetTimeZone(textTime);
                    return true;
                }
                catch (WebDriverException ex)
                {
                    return false;
                }
                catch (System.Collections.Generic.KeyNotFoundException ex)
                {
                    return true;
                }
                
                catch (Exception ex)
                {
                    if (ex.InnerException!=null && ex.InnerException.Message.IndexOf("The operation has timed out") != -1)
                    {
                        return false;
                    }
                    if (ex.InnerException.ToString().IndexOf("The operation has timed out") != -1)
                    {
                        return false;
                    }
                }
            
            return false;

        }
        public bool ConfirmWithLink(string link)
        {
            try
            {
                browser.Navigate().GoToUrl(link);
            }
            catch(OpenQA.Selenium.WebDriverException ex)
            {//если прокси сбросил подключение

            }
            ;
            try
            {
                IWebElement webElement =
                browser.FindElement(By.XPath("//p[@class='title']"));
                if (webElement != null)
                {
                    if (webElement.Text.IndexOf("Congratulations!") != -1)
                        return true;
                }
            }
            catch { }
            return false;
        }
        public bool CheckRegistrationWebPage()
        {
            //IWebElement webElement = null;
            //webElement = browser.FindElement(By.Id("chkSaveIp"));
            //((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('chkSaveIp').checked = false", webElement);
            try
            {
                if (!browser.FindElement(By.Id("chkAgree")).Enabled)
                    return false;
                if (browser.FindElement(By.Id("email_invalid")).Displayed)
                    return false;
                if (browser.FindElement(By.Id("birthDateMsg")).Displayed)
                    return false;
            }
            catch { return false; }
            try
            {
                if (browser.FindElement(By.TagName("iframe")) is null)
                    return false;
            }
            catch{ return false; }
            try
            {
                IWebElement webElement = null;
                webElement = browser.FindElement(By.TagName("iframe"));
                string src = "";
                if (webElement != null)
                {
                    src = webElement.GetAttribute("src");
                }
                string k = src.Remove(0, src.IndexOf("k=") + 2);
                k = k.Remove(k.IndexOf("&"));
            }
            catch { return false; }
            //try
            //{
            //    var webElement = browser.FindElement(By.Id("btnConfirm"));
            //    if (webElement != null)
            //    {
            //        //webElement.Click();
            //    }
            ////    catch (OpenQA.Selenium.ElementClickInterceptedException ex)
            ////{

            ////    try
            ////    {
            ////        iWebElement = browser.FindElement(By.XPath("//span[@class='box square']"));
            ////        iWebElement.Click();
            ////    }
            ////    catch (Exception e) { goto A; }
            ////}
            ////catch (OpenQA.Selenium.StaleElementReferenceException ex)
            ////{
            ////    goto A;
            ////}
            //}
            //catch { return false; }
            return true;
        }
        public void Close()
        {
            try
            {
                browser.Close();
                browser.Dispose();
            }
            catch { }
        }
        public int indexTry = 0;
        //account acc;
        public bool StartRegistration(account acc, Acc accProgram)
        {
            for(; ; )
            {
                break;
            }
            //this.acc = acc;
            string email=null;
            string userName = null;
            string password = null;
            string apiKey = null;
            string month = acc.date_of_birth.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;
            string day = acc.date_of_birth.Day.ToString();
            //if (day.Length == 1)
            //    day = "0" + day;
            string year = acc.date_of_birth.Year.ToString();

            using (NcSoftBase ncSoftBase = new NcSoftBase())
            {
                email = ncSoftBase.emails.Where(em => em.id.Equals(acc.email_id)).First().email1;
                apiKey = ncSoftBase.settings.First().captcha_api_key;
            }
            userName = acc.display_name;
            password = acc.password_;



            //try
            //{
            //    browser.Navigate().GoToUrl(url);
            //}
            //catch { }

            //recaptcha();


            
            //go google
            //browser.Navigate().GoToUrl("https://www.google.com.ua/");
            IWebElement iWebElement=null;

            //browser.Navigate().GoToUrl("https://whoer.net/ru");
            //;
            //browser.Navigate().GoToUrl("http://f.vision/");
            //;
            string startText="";
            indexTry = 0;
            A:
            //return true;
            indexTry++;
            if(indexTry>3)
            {
                return false;
            }
            startText = "[" + indexTry.ToString() + "] ";
            accProgram.StatusText = startText+"Получение страницы.";
            if (browser.Url.Equals("https://account.ncsoft.com/signup/index?serviceCode=13#"))
                browser.Navigate().Refresh();
            try
            {
                browser.Navigate().GoToUrl(url);
            }
            catch { }

            
            for (int i = 0;i<9 ;i++ )
            {
                if (CheckRegistrationWebPage())
                    break;
                try
                {
                    browser.Navigate().Refresh();
                }
                catch { }
                if(i==8)
                {
                    accProgram.StatusText = "Даже не получилось зайти на страницу (8 перезагрузок, не все элементы загружаются)";
                    accProgram.Percentage = 0;
                    return false;
                }
            }

           

            //iWebElement = browser.FindElement(By.XPath("//input[@class='gLFyf gsfi']"));
            if (true)//iWebElement != null)
            {
                ////insert question
                //iWebElement.SendKeys("nc Account registration");
                //iWebElement.SendKeys("\r\n");
                //iWebElement = browser.FindElement(By.XPath("//input[@name='btnK']"));
                if(true)//iWebElement!=null)
                {
                //    //click search button
                //    ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementsByName('btnK')[0].click()", iWebElement);
                //    Thread.Sleep(5000);

                //    //((IJavaScriptExecutor)browser).ExecuteScript("document.evaluate('//div[@id=\"search\"]//a[@href]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click();", iWebElement);
                //    iWebElement = browser.FindElement(By.Id("ires"));
                //    if (iWebElement != null)
                //    {
                //        iWebElement = iWebElement.FindElement(By.TagName("a"));
                //        if (iWebElement != null)
                //        {
                //            try
                //            {
                //                iWebElement.Click();
                //            }
                //            catch { }
                //        }
                        
                //    }
                    Thread.Sleep(500);
                    //id= loginName
                    //document.getElementById('loginName').value='df'
                    accProgram.StatusText = startText+"Ввод email";
                    int j = 0;
                    for (j=0;j<8 ;j++ )
                    try
                    {
                        iWebElement = browser.FindElement(By.Id("loginName"));
                            break;
                    }
                    catch { Thread.Sleep(600); }
                    if (j == 8)
                    {
                        accProgram.StatusText = "Ошибка при вводе данных";
                        accProgram.Percentage = 0;
                        return false;
                    }
                    ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('loginName').value='"+email+"'", iWebElement);
                    accProgram.StatusText = startText+"Email введен";
                    ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('password').value='" + password + "'", iWebElement);
                    accProgram.StatusText = startText + "Пароль введен";
                    ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('passwordRepeat').value='" + password + "'", iWebElement);
                    accProgram.StatusText = startText + "Повтор пароля введен";
                    //id = userName
                    //проверка правильности логина - очень важно
                    for (; ; )
                    {
                        iWebElement = browser.FindElement(By.Id("userName"));
                        ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('userName').value='" + userName + "'", iWebElement);
                        iWebElement.Click();
                        var el = browser.FindElement(By.Id("displayname_invalid"));
                        if (el != null && !el.GetAttribute("class").Equals("msg"))
                        {
                            accProgram.Percentage = 0;
                            accProgram.StatusText = "This display name is unavailable.";
                            userName = accProgram.ChangeName();
                            iWebElement = browser.FindElement(By.Id("userName"));
                            iWebElement.Click();
                            
                            continue;
                        }
                        el = browser.FindElement(By.Id("displayname_banned"));
                        if (el != null && !el.GetAttribute("class").Equals("msg"))
                        {
                            accProgram.Percentage = 0;
                            accProgram.StatusText = "This display name cannot be used.";
                            userName = accProgram.ChangeName();
                            iWebElement = browser.FindElement(By.Id("userName"));
                            iWebElement.Click();
                            continue;
                            //return false;
                        }
                        break;
                    }

                    accProgram.StatusText = startText + "Логин введен";
                    IWebElement selectBox = null;
                    SelectElement selectElement = null;

                    //передвижение, чтобы поля были в области видимости
                    iWebElement = browser.FindElement(By.Id("birthMonth"));
                    ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('birthMonth').scrollIntoView()", iWebElement);
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            selectBox = browser.FindElement(By.XPath("//select[@id='birthMonth']"));
                            selectElement = new SelectElement(selectBox);
                            selectElement.SelectByValue(month);
                            accProgram.StatusText = startText + "Месяц введен";
                            break;
                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(600);
                        }
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            selectBox = browser.FindElement(By.XPath("//select[@id='birthDay']"));
                            selectElement = new SelectElement(selectBox);
                            selectElement.SelectByValue(day);
                            accProgram.StatusText = startText + "День введен";
                            break;
                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(600);
                        }
                    }

                    //birthYear
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            selectBox = browser.FindElement(By.XPath("//select[@id='birthYear']"));
                            selectElement = new SelectElement(selectBox);
                            selectElement.SelectByValue(year);
                            accProgram.StatusText = startText + "Год введен";
                            break;
                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(600);
                        }
                    }

                    //id = chkSaveIp
                    iWebElement = browser.FindElement(By.Id("chkSaveIp"));
                    if (iWebElement != null)
                    {
                        //browser.FindElement(By.Id("chkSaveIp")).Text
                        for (int i = 0; i < 3; i++)
                        {
                            //chkNews
                            ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('chkNews').checked = true", iWebElement);

                            ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('chkSaveIp').checked = false", iWebElement);
                            //chkAgree
                            ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('chkAgree').checked = true", iWebElement);
                            accProgram.StatusText = startText + "Чекбоксы проставлены";

                            if (browser.FindElement(By.Id("chkAgree")).Selected==true && browser.FindElement(By.Id("chkSaveIp")).Selected == false&& browser.FindElement(By.Id("chkNews")).Selected==true)
                                break;
                        }
                    }
                    //recaptcha();
                }
            }
            B:
            accProgram.Percentage = 50;
            accProgram.StatusText = startText + "Определение и ввод капчи";
            //return;
            try
            {
                rucaptcha(apiKey, accProgram);
            }
            catch(Exception ex)
            {
                accProgram.StatusText = startText + "Ошибка при вводе капчи";
                return false;
            }
            accProgram.StatusText = startText + "Капча введена";
            accProgram.Percentage = 70;
            try
            {
                iWebElement = browser.FindElement(By.Id("btnConfirm"));
            }
            catch( Exception ex)
            {
                accProgram.Percentage = 0;
                accProgram.StatusText = "После ввода капчи не получилось обнаружить кнопку";
                return false;
            }
            accProgram.StatusText = startText + "Отправка формы";
            if (iWebElement != null)
            {
                //iWebElement.Submit();
                for(int i=0;i<40;i++ )
                {
                    try
                    {
                        accProgram.StatusText = startText + " Click - " + i.ToString() + " Отправка формы";
                        IWebElement webElement2 = browser.FindElement(By.Id("topHeader"));
                        if (webElement2 != null)
                        {
                            webElement2 = browser.FindElement(By.Id("contentWrap"));
                            string text = webElement2.Text;

                            if (text.IndexOf("Email verification failed.") != -1)
                            {
                                //reristration again
                                goto A;
                            }
                            if (text.IndexOf("Email Address") != -1)
                                ;// continue;

                            if (text.IndexOf("A verification email has been sent to the email address below.") != -1)
                            {
                                return true;
                            }
                        }
                    }
                    catch { }
                    try
                    {
                        //document.getElementById('btnConfirm').click()
                        //iWebElement.Click();
                        ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('btnConfirm').click()", iWebElement);
                        Thread.Sleep(800);
                        try
                        {
                            var el = browser.FindElement(By.Id("email_existed"));
                            if(el!=null&& !el.GetAttribute("class").Equals("msg"))
                            {
                                accProgram.Percentage = 0;
                                accProgram.StatusText = "Email уже зарегистрирован.";
                                return false;
                            }
                            el = browser.FindElement(By.Id("displayname_invalid"));
                            if (el != null && !el.GetAttribute("class").Equals("msg"))
                            {
                                accProgram.Percentage = 0;
                                accProgram.StatusText = "This display name is unavailable.";
                                userName = accProgram.ChangeName();
                                iWebElement = browser.FindElement(By.Id("userName"));
                                iWebElement.Click();
                                ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('userName').value='" + userName + "'", iWebElement);
                                accProgram.StatusText = startText + "Имя введено";
                                goto B;
                                //return false;
                            }
                            el = browser.FindElement(By.Id("displayname_banned"));
                            if (el != null && !el.GetAttribute("class").Equals("msg"))
                            {
                                accProgram.Percentage = 0;
                                accProgram.StatusText = "This display name cannot be used.";
                                userName = accProgram.ChangeName();
                                iWebElement = browser.FindElement(By.Id("userName"));
                                iWebElement.Click();
                                ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('userName').value='" + userName + "'", iWebElement);
                                accProgram.StatusText = startText + "Имя введено";
                                goto B;
                                //return false;
                            }
                            for (; ; )
                            {
                                break;
                            }
                        }
                        catch { }

                    }
                    catch(OpenQA.Selenium.ElementClickInterceptedException ex)
                    {
                        
                        try
                        {
                            iWebElement = browser.FindElement(By.XPath("//span[@class='box square']"));
                            iWebElement.Click();
                        }
                        catch(Exception e) { goto A; }
                    }
                    catch(OpenQA.Selenium.StaleElementReferenceException ex)
                    {
                        goto A;
                    }
                    catch (Exception ex)
                    {
                        accProgram.StatusText = startText + ex.Message;
                        Thread.Sleep(1000);
                        try
                        {
                            var webElement2 = browser.FindElement(By.Id("contentWrap"));
                            string text = webElement2.Text;

                            if (text.IndexOf("Email verification failed.") != -1)
                            {
                                //reristration again
                                goto A;
                            }
                            if (text.IndexOf("Email Address") != -1)
                                ;// continue;

                            if (text.IndexOf("A verification email has been sent to the email address below.") != -1)
                            {
                                return true;
                            }
                        }
                        catch { break; }
                    }
                    Thread.Sleep(500);
                }
                accProgram.Percentage = 0;
                accProgram.StatusText = "Не получилось перейти на следующую страницу";
                return false;
            }
            //iWebElement = browser.FindElement(By.Id(""));
            //if (iWebElement != null)
            //{
                
            //    //confirm from email
            //}
            return false;
        }
        
        public bool ResentVerificationEmail()
        {
            try
            {
                var element = browser.FindElement(By.XPath("//span[@class='button resend']"));
                ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementsByClassName('button resend')[0].click()", element);
                Thread.Sleep(600);
                for(int i = 0; i < 50; i++)
                {
                    try
                    {
                        if (browser.FindElement(By.XPath("//div[@class='msgTop error']")).Displayed)
                            return true;
                        ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementsByClassName('button resend')[0].click()", element);
                        Thread.Sleep(600);
                    }
                    catch { return true; }
                    
                }
            }
            catch { return false; }
            for (; ; )
            {
                break;
            }
            return false;
        }
        //public bool recaptcha()
        //{
        //    //class= recaptcha-checkbox-checkmark
        //    //span id = recaptcha-anchor
        //    IWebElement iWebElement = browser.FindElement(By.Id("birthYear"));
        //    //document.getElementById('recaptcha-anchor-label').scrollIntoView();
        //    ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('birthYear').scrollIntoView();", iWebElement);
        //    browser.SwitchTo().Frame(browser.FindElement(By.TagName("iframe")));

        //    iWebElement = browser.FindElement(By.ClassName("recaptcha-checkbox-checkmark"));
        //    if(iWebElement!=null)
        //    {
        //        iWebElement.Click();
        //    }
        //    browser.SwitchTo().DefaultContent();
        //    //frame with captcha
        //    browser.SwitchTo().Frame(browser.FindElement(By.XPath("//iframe[@title='recaptcha challenge']")));
        //    //class = rc-imageselect-desc
        //    //iWebElement = browser.FindElement(By.ClassName("rc-imageselect-desc"))
        //    //получить данные на что кликать
        //    iWebElement = null;
        //    for(int i =0;i<5;i++)
        //    {
        //        try
        //        {
        //            iWebElement = browser.FindElement(By.TagName("strong"));
        //        }
        //        catch
        //        {

        //        }
        //        Thread.Sleep(6000);
        //    }
        //    string whatClick = "";
        //    if (iWebElement != null)
        //    {
        //        whatClick = iWebElement.Text;
        //    }
        //    //кликпо картинке
        //    //iWebElement = browser.FindElement(By.ClassName("rc-image-tile-target"));
        //    //if (iWebElement != null)
        //    //{
        //    //    iWebElement.Click();
        //    //}
        //    A:
        //    //весь списоккартинок
        //    var v = browser.FindElements(By.ClassName("rc-image-tile-target"));
        //    //определение колличества
        //    int rowCount=3;
        //    int columnCount=3;
        //    if(v.Count==16)
        //    {
        //        rowCount = 4;
        //        columnCount = 4;
        //    }
        //    if(v.Count == 9)
        //    {
        //        rowCount = 3;
        //        columnCount = 3;
        //    }
        //    //выполнить сохранение картинки
        //    var base64string = ((IJavaScriptExecutor)browser).ExecuteScript(@"
        //        var c = document.createElement('canvas');
        //        var ctx = c.getContext('2d');
        //        var img = document.getElementsByClassName('rc-image-tile-" + rowCount.ToString() + columnCount.ToString() +
        //        "')[0];" +
        //        "c.height=img.height;" +
        //        "c.width=img.width;" +
        //        "ctx.drawImage(img, 0, 0,img.width, img.height);" +
        //        "var base64String = c.toDataURL();" +
        //        "return base64String;");

        //    var base64 = ((string)base64string).Split(',').Last();
        //    string guid = Guid.NewGuid().ToString();
        //    string imagePath = @"Images\" + "image" + guid + ".png";
        //    string imageResultPath = @"Images\" + "imageResult" + guid + ".png";
        //    using (var stream = new MemoryStream(Convert.FromBase64String(base64)))
        //    {
        //        using (var bitmap = new Bitmap(stream))
        //        {
        //            //var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageName.png");
        //            bitmap.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
        //        }
        //    }
        //    dravingGrid(columnCount, rowCount, imagePath, imageResultPath);
        //    Captcha captcha = new Captcha(whatClick, imageResultPath);
        //    captcha.ShowDialog();
        //    var listCaptchaNumbers = captcha.NumbersImages;

        //    var elements = browser.FindElements(By.ClassName("rc-image-tile-target"));
        //    for (int i = 0; i < listCaptchaNumbers.Count; i++)
        //        elements[listCaptchaNumbers[i]-1].Click();
        //    //кнопка которую нажимать
        //    iWebElement = browser.FindElement(By.Id("recaptcha-verify-button"));
        //    string buttonText = "";
        //    if(iWebElement!=null)
        //    {
        //        buttonText = iWebElement.Text;
        //    }
        //    if(buttonText.Equals("NEXT"))
        //    {
        //        iWebElement.Click();
        //        goto A;
        //    }
        //    //rc-image-tile-11
        //    //элементы которые появятся в картинках на новых местах
        //    elements = browser.FindElements(By.ClassName("rc-image-tile-11"));

        //    List<string> listPathImages = new List<string>();
        //    for(int i = 0; i < elements.Count; i++)
        //    {

        //        base64string = ((IJavaScriptExecutor)browser).ExecuteScript(@"
        //        var c = document.createElement('canvas');
        //        var ctx = c.getContext('2d');
        //        var img = document.getElementsByClassName('rc-image-tile-11" +
        //        "')["+i.ToString()+"];" +
        //        "c.height=img.height;" +
        //        "c.width=img.width;" +
        //        "ctx.drawImage(img, 0, 0,img.width, img.height);" +
        //        "var base64String = c.toDataURL();" +
        //        "return base64String;");

        //        base64 = ((string)base64string).Split(',').Last();
        //        guid = Guid.NewGuid().ToString();
        //        imagePath = @"Images\" + "imageOne" + guid + ".png";
        //        listPathImages.Add(imagePath);
        //        using (var stream = new MemoryStream(Convert.FromBase64String(base64)))
        //        {
        //            using (var bitmap = new Bitmap(stream))
        //            {
        //                //var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageName.png");
        //                bitmap.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
        //            }
        //        }

        //    }




        //    listCaptchaNumbers.Sort();
        //    for (int i=0;i< listCaptchaNumbers.Count; i++)
        //    {

        //    }
        //    //if(iWebElement!=null)
        //    //{
        //    //    iWebElement.Click();
        //    //}
        //    ;
        //    //((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('chkAgree').click()", iWebElement);
        //    return true;
        //}
        public void dravingGrid(int countColumns,int countRows, string imagePath, string imageResultPath)
        {
            Bitmap bitmap = new Bitmap(imagePath);
            Pen pen = new Pen(Color.White, 3);
            int x1 = 100;
            int y1 = 100;
            int x2 = 500;
            int y2 = 100;
            
            for(int i = 1; i < countColumns; i++)
            {
                x1 = i*bitmap.Width/ countColumns;
                y1 = 0;
                x2 = x1;
                y2 = bitmap.Height;
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
            
            for(int i = 1; i < countRows; i++)
            {
                x1 = 0;
                y1 = i * bitmap.Height / countRows;
                x2 = bitmap.Width;
                y2 = y1;
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
            int number = 1;
            Font font = new Font(new FontFamily("Arial"), 14);
            ;
            Brush brush = new SolidBrush(Color.White);
            Point point = new Point();
            for (int j = 0; j < countRows; j++)
            {
                for (int i = 0; i < countColumns; i++) 
                {
                    point = new Point((bitmap.Width / countColumns) * i + 2, (bitmap.Height / countRows) * j + 2); 
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        Rectangle rectangle = new Rectangle(point, new Size(17, 17));
                        //graphics.DrawRectangle(new Pen(Color.Black, 3), rectangle);
                        graphics.FillRectangle(new SolidBrush(Color.Black), rectangle);
                        graphics.DrawString(number.ToString().ToString(),font, brush, point);
                    }
                    number++;
                }
            }
            // Draw line to screen.
            bitmap.Save(imageResultPath, System.Drawing.Imaging.ImageFormat.Png);
        }
        //public void testCaptcha()
        //{
        //    Captcha captcha = new Captcha("sdfsdf", @"Images\" + "imageResult3f470dc2-e17e-4a9e-8cb6-cbc261ef79ef" + ".png");
        //    captcha.ShowDialog();
        //    ;
        //}
        //string apiKey = "d2046ad540a4366975d2893791dc6d9e";
        public void rucaptcha(string apiKey, Acc accProgram)
        {
            IWebElement webElement = null;
            webElement = browser.FindElement(By.TagName("iframe"));
            string src = "";
            if(webElement!=null)
            {
                src = webElement.GetAttribute("src");
            }
            string k = src.Remove(0, src.IndexOf("k=")+2);
            k = k.Remove(k.IndexOf("&"));
            //http://rucaptcha.com/in.php?key=1abc234de56fab7c89012d34e56fa7b8&method=userrecaptcha&googlekey=6Le-wvkSVVABCPBMRTvw0Q4Muexq1bi0DJwx_mJ-&pageurl=http://mysite.com/page/with/recaptcha?appear=1&here=now
            HttpClient httpClient=new HttpClient();
            B:
            string respond = "";
            try
            {
                accProgram.StatusText = "Запрос к сервису капчи ";
                respond = httpClient.GetStringAsync("http://rucaptcha.com/in.php?key=" + apiKey + "&method=userrecaptcha&googlekey=" + k + "&pageurl=https://account.ncsoft.com/signup/index?serviceCode=13#").GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {

                accProgram.StatusText = "Ошибка при отправке запроса: "+ex.Message;
                Thread.Sleep(6000);
                goto B;
            }
            string idCaptcha = "";
            if (respond.IndexOf("OK") != -1)
            {
                idCaptcha = respond.Remove(0, respond.IndexOf('|') + 1);
            }

            for (; ; )
            {
                Thread.Sleep(6000);
                respond = httpClient.GetStringAsync("http://rucaptcha.com/res.php?key=" + apiKey + "&action=get&id="+idCaptcha).GetAwaiter().GetResult();
                if (respond.IndexOf("CAPCHA_NOT_READY") == -1)
                {
                    break;
                }
                if (respond.IndexOf("ERROR_WRONG_CAPTCHA_ID") != -1)
                {
                    accProgram.StatusText = respond;
                    throw new Exception("ERROR_WRONG_CAPTCHA_ID");
                }
                if (respond.Equals("ERROR_CAPTCHA_UNSOLVABLE"))
                {
                    accProgram.StatusText = respond;
                    throw new Exception("ERROR_CAPTCHA_UNSOLVABLE");
                }

            }
            
            string result = respond.Split(new char[] { '|' })[1];
            webElement = browser.FindElement(By.Id("g-recaptcha-response"));
            ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('g-recaptcha-response').value='"+result+"';", webElement);
            ;
            //webElement.
            //webElement.SendKeys(result);
        }
        public bool ConfirmFromEmail(string url)
        {
            browser.Navigate().GoToUrl(url);
            var iWebElement = browser.FindElement(By.Id(""));
            if (iWebElement != null)
            {
                return true;
            }
            return false;
        }
    }
}
