using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.InfraCore.Email
{
    public abstract class EmailStrategy
    {
        public abstract void SetSender(string fromMail, string password);
        public abstract void Send(string toEmail, string subject, string htmlContent, ref string errorMessage);
    }
}
