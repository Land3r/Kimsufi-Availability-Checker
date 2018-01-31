using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server;
using System.Net.Mail;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Email.EmailAlertService
{
    /// <summary>
    /// EmailAlert Service.
    /// Used to send email alerts.
    /// </summary>
    public class EmailAlertService : IEmailAlertService
    {
        /// <summary>
        /// The internal email service used.
        /// </summary>
        private IEmailService emailService = new EmailService();

        /// <summary>
        /// Sends the alert associated to the provided <see cref="ServerStatus"/> to the email provided.
        /// </summary>
        /// <param name="serverStatus">The <see cref="ServerStatus"/> to send alert for.</param>
        /// <param name="email">The email address of the recipient.</param>
        public void SendEmailAlert(ServerStatus serverStatus, string email)
        {
            MailMessage message = new MailMessage(email, email, string.Format("Server {0} is now available", serverStatus.Name), string.Format("OVH Kimsufi server {0} is now available for {1}", serverStatus.Name, serverStatus.Price));
            emailService.SendEmail(message);
        }
    }
}
