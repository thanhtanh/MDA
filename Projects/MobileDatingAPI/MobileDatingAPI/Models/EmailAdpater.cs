using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace MobileDatingAPI.Models
{

    public class EmailAdpater : IDisposable
    {

        #region Settings

        private const string Username = "vpnseo2014.02@gmail.com";
        private const string SenderAddress = "vpnseo2014.02@gmail.com";
        private const string Password = "vpnseo02";
        private const string EmailHost = "smtp.gmail.com";
        private const int EmailPort = 587;
        private const string SenderName = "Mobile Dating Account";

        #endregion

        protected SmtpClient SmtpClient { get; set; }

        public EmailAdpater() {
            this.SmtpClient = new SmtpClient()
            {
                Host = EmailHost,
                Port = EmailPort,
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = true,
            };
        }

        public void SendEmail(string subject, string body, string pAddressTo, string[] pAddressCc = null, string[] pAddressBcc = null)
        {
            MailMessage message = new MailMessage();

            #region Addresses

            message.From = new MailAddress(SenderAddress, SenderName);
            message.To.Add(new MailAddress(pAddressTo));

            if (pAddressCc != null)
            {
                foreach (var cc in pAddressCc)
                {
                    message.CC.Add(new MailAddress(cc));
                }
            }

            if (pAddressBcc != null)
            {
                foreach (var bcc in pAddressBcc)
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }
            }

            #endregion

            #region Content

            message.Subject = subject;
            message.IsBodyHtml = true;

            var view = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
            message.AlternateViews.Add(view);

            #endregion

            this.SmtpClient.Send(message);
        }

        public void SendEmailAsync(string subject, string body, string pAddressTo, string[] pAddressCc = null, string[] pAddressBcc = null)
        {
            MailMessage message = new MailMessage();

            #region Addresses

            message.From = new MailAddress(SenderAddress, SenderName);
            message.To.Add(new MailAddress(pAddressTo));

            if (pAddressCc != null)
            {
                foreach (var cc in pAddressCc)
                {
                    message.CC.Add(new MailAddress(cc));
                }
            }

            if (pAddressBcc != null)
            {
                foreach (var bcc in pAddressBcc)
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }
            }

            #endregion

            #region Content

            message.Subject = subject;
            message.IsBodyHtml = true;

            var view = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
            message.AlternateViews.Add(view);

            #endregion

            this.SmtpClient.SendAsync(message, null);
        }

        public void Dispose()
        {
            this.SmtpClient.Dispose();
        }
    }

}