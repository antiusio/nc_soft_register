using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Registration
{
    public static class ConfirmEmail
    {
        public static bool GmailConfirm(string email, string password, string confirmEmail)
        {
            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("intl.accept_languages", "en-us");
            var fireFoxOptions = new FirefoxOptions();
            fireFoxOptions.Profile = profile;
            IWebDriver browser = new FirefoxDriver(fireFoxOptions);

            browser.Navigate().GoToUrl("https://www.google.com/gmail/");
            //browser.Navigate().GoToUrl("https://accounts.google.com/AccountChooser?service=mail&continue=https://mail.google.com/mail/");
            //input id = identifierId
            IWebElement webElement = null;
            webElement = browser.FindElement(By.Id("identifierId"));
            try
            {
                if (webElement != null)
                {
                    webElement.SendKeys(email);
                    webElement = browser.FindElement(By.Id("identifierNext"));
                    if (webElement != null)
                    {
                        webElement.Click();
                        Thread.Sleep(6000);
                        webElement = browser.FindElement(By.XPath("//input[@type='password']"));
                        if (webElement != null)
                        {
                            //Thread.Sleep(6000);
                            webElement.SendKeys(password);
                            webElement = browser.FindElement(By.Id("passwordNext"));
                            if (webElement != null)
                            {
                                webElement.Click();
                                Thread.Sleep(6000);
                                webElement = browser.FindElement(By.XPath("//li/div[@role='button']"));
                                if (webElement != null)
                                {
                                    webElement.Click();
                                    Thread.Sleep(6000);
                                    webElement = browser.FindElement(By.Id("identifierId"));
                                    if (webElement != null)
                                    {
                                        webElement.SendKeys(confirmEmail);
                                        webElement = browser.FindElement(By.XPath("//div[@role='button']"));
                                        if (webElement != null)
                                        {
                                            webElement.Click();
                                            Thread.Sleep(6000);
                                            webElement = browser.FindElements(By.XPath("//div[@role='button']"))[1];
                                            if (webElement != null)
                                            {
                                                webElement.Click();
                                                browser.Close();
                                                browser.Dispose();
                                                return true;
                                            }
                                        }


                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return false;
        }
        public static string GetConfirmLink2(string email, string password, string confirmEmail)
        {
            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("intl.accept_languages", "en-us");
            var fireFoxOptions = new FirefoxOptions();
            fireFoxOptions.Profile = profile;
            IWebDriver browser = new FirefoxDriver(fireFoxOptions);

            browser.Navigate().GoToUrl("https://www.google.com/gmail/");
            //browser.Navigate().GoToUrl("https://accounts.google.com/AccountChooser?service=mail&continue=https://mail.google.com/mail/");
            //input id = identifierId
            IWebElement webElement = null;
            webElement = browser.FindElement(By.Id("identifierId"));
            try
            {
                if (webElement != null)
                {
                    webElement.SendKeys(email);
                    webElement = browser.FindElement(By.Id("identifierNext"));
                    if (webElement != null)
                    {
                        webElement.Click();
                        Thread.Sleep(6000);
                        webElement = browser.FindElement(By.XPath("//input[@type='password']"));
                        if (webElement != null)
                        {
                            //Thread.Sleep(6000);
                            webElement.SendKeys(password);
                            webElement = browser.FindElement(By.Id("passwordNext"));
                            if (webElement != null)
                            {
                                webElement.Click();
                                Thread.Sleep(6000);
                                webElement = browser.FindElement(By.XPath("//li/div[@role='button']"));
                                if (webElement != null)
                                {
                                    webElement.Click();
                                    Thread.Sleep(6000);
                                    webElement = browser.FindElement(By.Id("identifierId"));
                                    if (webElement != null)
                                    {
                                        webElement.SendKeys(confirmEmail);
                                        webElement = browser.FindElement(By.XPath("//div[@role='button']"));
                                        if (webElement != null)
                                        {
                                            webElement.Click();
                                            Thread.Sleep(6000);
                                            webElement = browser.FindElements(By.XPath("//div[@role='button']"))[1];
                                            if (webElement != null)
                                            {
                                                webElement.Click();
                                                //browser.Close();
                                                //browser.Dispose();
                                                ;
                                                //browser.
                                                //return true;
                                            }
                                        }


                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            bool f=CheckEmails(browser);
            if (f)
            {
                for (int i=0;i<2 ;i++ )
                {
                    IWebElement element = browser.FindElement(By.ClassName("UI"));
                    var emails = element.FindElements(By.TagName("tr"));
                    ;
                    foreach (var em in emails)
                    {
                        if (em.Text.IndexOf("[NCSOFT] Verify your email address") != -1)
                        {
                            //((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('chkNews').checked = true", element);
                            //em.Click();
                            //((IJavaScriptExecutor)browser).ExecuteScript("document.evaluate('//div[@id=\"search\"]//a[@href]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click();", em);
                            ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('" + em.GetAttribute("id") + "').click()", em);
                            Thread.Sleep(3000);
                            HtmlDocument doc = new HtmlDocument();
                            for (; ; )
                            {
                                if (browser.Title.IndexOf("[NCSOFT] Verify your email address") != -1)
                                {
                                    element = browser.FindElement(By.XPath("//table[@style='width:700px;table-layout:fixed;margin:0 auto;padding:0;border:0;font-family:Lucida Sans Unicode,Arial,Verdana,sans-serif']"));
                                    //doc.LoadHtml(element.inn);
                                    //return doc.DocumentNode.SelectNodes("//a")[1].GetAttributeValue("href", "");
                                    return element.FindElements(By.TagName("a"))[1].GetAttribute("href");
                                }
                                Thread.Sleep(1500);
                            }
                        }
                    }
                    Thread.Sleep(6000);
                    browser.Navigate().Refresh();
                }
            }
            return "";
        }
        private static bool CheckEmails(IWebDriver driver)
        {
            IWebElement webElement=null;
            try
            {
                webElement = driver.FindElement(By.Id("gtn-roster-iframe-id"));
                driver.FindElement(By.ClassName("UI"));
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
