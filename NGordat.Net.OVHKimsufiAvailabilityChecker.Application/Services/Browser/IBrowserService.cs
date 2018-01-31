using NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Browser
{
    public interface IBrowserService
    {
        /// <summary>
        /// Navigates to the specified Uri.
        /// </summary>
        /// <param name="url">The url where to navigate.</param>
        void LoadUrl(string url);

        /// <summary>
        /// Sets the list of <see cref="ServerStatus"/> shown on the interface.
        /// </summary>
        /// <param name="serverStatus"></param>
        void SetData(IList<ServerStatus> serverStatus);
    }
}
