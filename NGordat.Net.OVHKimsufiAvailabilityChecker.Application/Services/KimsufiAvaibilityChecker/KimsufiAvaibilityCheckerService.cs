using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.Browser;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.KimsufiAvaibilityChecker
{
    public class KimsufiAvaibilityCheckerService : IKimsufiAvaibilityCheckerService
    {
        /// <summary>
        /// The <see cref="IKimsufiAvaibilityCheckerServiceListener"/> that will listen to the events sent by this class.
        /// </summary>
        private IKimsufiAvaibilityCheckerServiceListener listener;

        /// <summary>
        /// The <see cref="IBrowserService"/> used to retrieve the data.
        /// </summary>
        private IBrowserService browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="KimsufiAvaibilityChecker"/> class with no event listener.
        /// </summary>
        public KimsufiAvaibilityCheckerService() : this(null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KimsufiAvaibilityChecker"/> class with a specific event listener.
        /// </summary>
        /// <param name="listener">The event listener used by this class.</param>
        public KimsufiAvaibilityCheckerService(IKimsufiAvaibilityCheckerServiceListener listener, IBrowserService browser)
        {
            this.listener = listener;
            this.browser = browser;
        }

        #region IKimsufiAvaibilityCheckerService

        public ICollection<ServerStatus> LatestStatus => throw new NotImplementedException();

        public ICollection<ICollection<ServerStatus>> Status => throw new NotImplementedException();

        public void RetrieveKimsufiData(string kimsufiUri)
        {
            if (browser != null)
            {
                browser.LoadUrl(kimsufiUri);
            }
            else
            {
                throw new ApplicationException(string.Format("Tried to navigate to {0}, but the browser isn't initialized.", kimsufiUri));
            }
        }

        #endregion IKimsufiAvaibilityCheckerService
    }
}
