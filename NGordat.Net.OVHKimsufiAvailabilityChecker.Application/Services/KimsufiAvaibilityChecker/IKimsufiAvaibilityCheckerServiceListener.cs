using NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.KimsufiAvaibilityChecker
{
    public interface IKimsufiAvaibilityCheckerServiceListener
    {
        /// <summary>
        /// Sets the status of the program.
        /// </summary>
        /// <param name="status"></param>
        void SetProgramStatus(string status);

        /// <summary>
        /// Sets the list of <see cref="KimsufiServerStatus"/>.
        /// </summary>
        /// <param name="serverStatus">The list of <see cref="KimsufiServerStatus"/>.</param>
        void SetKimsufiServerStatus(IList<ServerStatus> serverStatus);
    }
}
