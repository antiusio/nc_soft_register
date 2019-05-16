using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailApi
{
    public static partial class Gmail
    {
        public static ReadOnlyCollection<Cookie> GetCookiesToLogIn(string email, string password, string confirmEmail)
        {
            return ConfirmEmail.GetCookiesToLogIn(email,password,confirmEmail);
        }
        public static string GetConfirmLink2(string email, string password, string confirmEmail)
        {
            return ConfirmEmail.GetConfirmLink2(email, password,confirmEmail);
            //string xmlText = GetEmailsString(email, password, confirmEmail);


            


            
            return "";
        }

        public static IEnumerable<string> GetAllEmails2(string email, string password, string confirmEmail)
        {

            //var allEmails = mailRepository.GetAllMails();

            //return allEmails;
            return null;
            
        }
    }
}
