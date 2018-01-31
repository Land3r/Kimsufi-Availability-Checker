using NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.KimsufiAvaibilityChecker
{
    /// <summary>
    /// KimsufiAvaibilityChecker Service Interface.
    /// </summary>
    public interface IKimsufiAvaibilityCheckerService
    {
        /// <summary>
        /// Triggers the retrieval of Data
        /// </summary>
        void RetrieveKimsufiData(string kimsufiUri);

        /// <summary>
        /// The KimsufiServerStatus retrieved by the previous query
        /// </summary>
        ICollection<ServerStatus> LatestStatus { get; }

        /// <summary>
        /// The latest 5 retrieval of status to have some historic
        /// </summary>
        ICollection<ICollection<ServerStatus>> Status { get; }
    }
}
