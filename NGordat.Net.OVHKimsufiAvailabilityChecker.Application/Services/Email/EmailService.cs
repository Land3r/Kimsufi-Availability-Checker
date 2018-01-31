using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Email
{
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Gets or sets the server used by the smtp.
        /// </summary>
        static public string SmtpServer { get; set; }

        /// <summary>
        /// Gets or sets the port used by the smtp.
        /// </summary>
        static public int SmtpPort { get; set; }

        /// <summary>
        /// Gets or sets the username used by the smtp.
        /// </summary>
        static public string SmtpUsername { get; set; }

        /// <summary>
        /// Gets or sets the password used by the stmp.
        /// </summary>
        static public string SmtpPassword { get; set; }

        /// <summary>
        /// Gets or sets the sent from on the smtp.
        /// </summary>
        static public string SmtpEmailFrom { get; set; }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="message"></param>
        public void SendEmail(MailMessage message)
        {
            SmtpClient smtpClient = new SmtpClient(SmtpServer, SmtpPort);
            NetworkCredential credential = new NetworkCredential(SmtpUsername, SmtpPassword);
            smtpClient.Credentials = credential;
            smtpClient.EnableSsl = true;

            smtpClient.Send(message);
        }
    }
}
