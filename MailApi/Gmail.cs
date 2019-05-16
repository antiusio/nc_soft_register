using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using HtmlAgilityPack;
using System.Threading;

namespace MailApi
{
    public static partial class Gmail
    {
        public static string GetEmailsString(string email, string password,string confirmEmail)
        {
            for(int i=0;i<2;i++)
                try
                {
                    System.Net.WebClient objClient = new System.Net.WebClient();
                    string response;
                    //string title;
                    //string summary;

                    //Creating a new xml document
                    //XmlDocument doc = new XmlDocument();

                    //Logging in Gmail server to get data
                    objClient.Credentials = new System.Net.NetworkCredential(email, password);
                    //reading data and converting to string
                    response = Encoding.UTF8.GetString(
                               objClient.DownloadData(@"https://mail.google.com/mail/feed/atom"));

                    response = response.Replace(
                         @"<feed version=""0.3"" xmlns=""http://purl.org/atom/ns#"">", @"<feed>");

                    //loading into an XML so we can get information easily
                    //doc.LoadXml(response);

                    ////nr of emails
                    //var nr = doc.SelectSingleNode(@"/feed/fullcount").InnerText;

                    ////Reading the title and the summary for every email
                    //foreach (XmlNode node in doc.SelectNodes(@"/feed/entry"))
                    //{
                    //    title = node.SelectSingleNode("title").InnerText;
                    //    summary = node.SelectSingleNode("summary").InnerText;
                    //}
                    return response;
                }
                catch (Exception exe)
                {
                    //return null;
                    if (exe.Message.Equals("The remote server returned an error: (401) Unauthorized."))
                        ConfirmGEmail(email,password,confirmEmail);


                //MessageBox.Show("Check your network connection");
                }
            return null;
        }
        public static void ConfirmGEmail(string email, string password, string confirmEmail)
        {
            bool rez = ConfirmEmail.GmailConfirm(email, password, confirmEmail);
        }
        public static IEnumerable<string> GetAllEmails(string email, string password, string confirmEmail)
        {
            try
            {
                System.Net.WebClient objClient = new System.Net.WebClient();
                string response;
                
                objClient.Credentials = new System.Net.NetworkCredential(email, password);
                response = Encoding.UTF8.GetString(
                           objClient.DownloadData(@"https://mail.google.com/mail/feed/atom"));
            }
            catch (Exception exe)
            {
                //return null;
                if (exe.Message.Equals("The remote server returned an error: (401) Unauthorized."))
                    ConfirmGEmail(email, password, confirmEmail);


                //MessageBox.Show("Check your network connection");
            }
            var mailRepository = new MailRepository("imap.gmail.com", 993, true, email, password);
            var allEmails = mailRepository.GetAllMails();

            return allEmails;

            //Assert.IsTrue(allEmails.ToList().Any());
        }
        public static string GetConfirmLink(string email, string password, string confirmEmail)
        {
            //string xmlText = GetEmailsString(email, password, confirmEmail);

            
            for (int i=0;i<2;i++ )
            {
                var emails = GetAllEmails(email, password, confirmEmail);
                foreach (var emailText in emails)
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(emailText);
                    if (doc.DocumentNode.SelectSingleNode("//title")!=null&&doc.DocumentNode.SelectSingleNode("//title").InnerText.IndexOf("[NCSOFT] Verify your email address") != -1)
                    {
                        return doc.DocumentNode.SelectNodes("//a")[1].GetAttributeValue("href", "");
                    }
                }
                Thread.Sleep(5*60000);
            }


            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(xmlText);
            ////nr of emails
            //var nr = doc.SelectSingleNode(@"/feed/fullcount").InnerText;
            //string title;
            //string summary;
            ////Reading the title and the summary for every email
            //foreach (XmlNode node in doc.SelectNodes(@"/feed/entry"))
            //{
            //    title = node.SelectSingleNode("title").InnerText;
            //    summary = node.SelectSingleNode("summary").InnerText;
            //    if(title.Equals("[NCSOFT] Verify your email address"))
            //    {
            //        string link = node.SelectSingleNode("link").Attributes["href"].Value;
            //        ;
            //        System.Net.WebClient objClient = new System.Net.WebClient();
            //        string response;
            //        //string title;
            //        //string summary;

            //        //Creating a new xml document
            //        //XmlDocument doc = new XmlDocument();

            //        //Logging in Gmail server to get data
            //        objClient.Credentials = new System.Net.NetworkCredential(email, password);
            //        //reading data and converting to string
            //        //response = Encoding.UTF8.GetString(
            //                   //objClient.DownloadData(link));
            //        ;
            //        response = Encoding.UTF8.GetString(
            //                   objClient.DownloadData(@"https://mail.google.com/mail/feed/atom"));
            //        response = Encoding.UTF8.GetString(
            //                   objClient.DownloadData(link));
            //        ;
            //    }
            //}
            return "";
        }
    }
    public class MailRepository //: IMailRepository
    {
        private readonly string mailServer, login, password;
        private readonly int port;
        private readonly bool ssl;

        public MailRepository(string mailServer, int port, bool ssl, string login, string password)
        {
            this.mailServer = mailServer;
            this.port = port;
            this.ssl = ssl;
            this.login = login;
            this.password = password;
        }

        public IEnumerable<string> GetUnreadMails()
        {
            var messages = new List<string>();

            using (var client = new ImapClient())
            {
                client.Connect(mailServer, port, ssl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(login, password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var results = inbox.Search(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
                foreach (var uniqueId in results.UniqueIds)
                {
                    var message = inbox.GetMessage(uniqueId);

                    messages.Add(message.HtmlBody);

                    //Mark message as read
                    //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                }

                client.Disconnect(true);
            }

            return messages;
        }

        public IEnumerable<string> GetAllMails()
        {
            var messages = new List<string>();

            using (var client = new ImapClient())
            {
                client.Connect(mailServer, port, ssl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(login, password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var results = inbox.Search(SearchOptions.All, SearchQuery.NotSeen);
                foreach (var uniqueId in results.UniqueIds)
                {
                    var message = inbox.GetMessage(uniqueId);

                    messages.Add(message.HtmlBody);

                    //Mark message as read
                    //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                }

                client.Disconnect(true);
            }

            return messages;
        }

        

    }
}
