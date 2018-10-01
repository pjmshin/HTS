using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Configuration;


namespace SexyGuy.Tools
{


    public class MailManager
    {
        private MailMessage _mailMessage;
        private SmtpClient _smtpClient;

        public MailManager(string host, int port, string id, string pwd)
        {
            _smtpClient = new SmtpClient(host, port);
            _smtpClient.Credentials = new NetworkCredential(id, pwd);

            _mailMessage = new MailMessage();
            _mailMessage.IsBodyHtml = true;
            _mailMessage.Priority = MailPriority.Normal;
   
        }

        public string From
        {
            get { return _mailMessage.From==null ? string.Empty : _mailMessage.From.Address; }
            set { _mailMessage.From = new MailAddress(value);} 
        }

        public MailAddressCollection To
        {
            get { return _mailMessage.To}
           
        }

        public string Subject
        {
            get { return _mailMessage.Subject; }
            set { _mailMessage.Subject = value; }
        }

        public string Body
        {
            get { return _mailMessage.Body; }
            set { _mailMessage.Body = value; }
        }

        public void Send()
        {
            _smtpClient.Send(_mailMessage);

        }


        //public MailAddressCollection CC
        //{
        //    get { return _mailMessage.CC}
        //    set { _mailMessage.CC = value; }
        //}

        //public MailAddressCollection Bcc
        //{
        //    get { return _mailMessage.Bcc; }
        //    set { _mailMessage.Bcc = value; }
        //}


        #region Methods

        public static void Send(string from, string to, string subject, string content , string cc, string bcc)
        {
            if (String.IsNullOrEmpty(from))
                throw new ArgumentNullException("Sender is empty....");

            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException("To is empty....");
                

            string smtpHost= ConfigurationManager.AppSettings["SMTPHost"];
            int smtpPort = 0;

            if (ConfigurationManager.AppSettings["SMTPPort"] == null ||
                int.TryParse(ConfigurationManager.AppSettings["SMTPPort"], out smtpPort) == false)
                smtpPort = 25;

            String smtpId = ConfigurationManager.AppSettings["SMTPId"];
            String smtpPwd = ConfigurationManager.AppSettings["SMTPPassword"];

            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(to);
            message.CC.Add(cc);
            message.Bcc.Add(bcc);

            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = content;
            message.Priority = MailPriority.Normal;


            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Credentials = new NetworkCredential(smtpId, smtpPwd);

            smtpClient.Host = smtpHost;
            smtpClient.Port = smtpPort;
            smtpClient.Send(message);

        }

        public static void Send(string from, string to, string subject, string content)
        {
            Send(from, to, subject, content, null, null);
        }

        public static void Send(string to, string subject, string content)
        {

            string sender = ConfigurationManager.AppSettings["SMTPSender"];
            Send(sender,to,subject,content);

        }
        #endregion
    }
}
