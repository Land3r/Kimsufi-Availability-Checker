using NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Email.EmailAlertService
{
    /// <summary>
    /// EmailAlert Service Interface.
    /// Used to send email alerts.
    /// </summary>
    public interface IEmailAlertService
    {
        void SendEmailAlert(ServerStatus serverStatus, string email);
    }
}
