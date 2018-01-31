using System.Net.Mail;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(MailMessage message);
    }
}
