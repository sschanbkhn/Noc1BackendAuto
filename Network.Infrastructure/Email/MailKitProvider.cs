using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.InfraCore.Email
{
    public class MailKitProvider : EmailStrategy
    {
        private readonly MimeMessage message;
        private readonly SmtpClient smtp;
        private readonly string host;
        private readonly int port;
        private string fromMail;
        private string password;
        public MailKitProvider(string host, int port)
        {
            message = new MimeMessage();
            smtp = new SmtpClient();
            this.host = host;
            this.port = port;
        }
        public override void SetSender(string fromMail, string password)
        {
            this.fromMail = fromMail;
            this.password = password;
        }
        public override void Send(string toEmail, string subject, string htmlContent, ref string errorMessage)
        {
            try
            {
                message.From.Add(new MailboxAddress(fromMail, fromMail));
                message.To.Add(new MailboxAddress(toEmail, toEmail));
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = htmlContent
                };                
                smtp.Connect(host, port, false);
                smtp.Authenticate(fromMail, password);
                smtp.Send(message);
                smtp.Disconnect(true);                
            }
            catch (Exception ex) { errorMessage = ex.Message; }
        }
    }
}
