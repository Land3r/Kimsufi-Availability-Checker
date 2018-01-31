using log4net;
using Newtonsoft.Json;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Application.Services.KimsufiAvaibilityChecker;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Domain.Server;
using NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NGordat.Net.OVHKimsufiAvailabilityChecker.Presentation.Services.Browser
{
    /// <summary>
    /// Callback class for Javascript.
    /// Allows to call .NET methods from JS stack.
    /// </summary>
    public class BrowserCallbackForJs
    {
        /// <summary>
        /// The parent caller.
        /// </summary>
        private BrowserForm caller;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserCallbackJs"/> class.
        /// </summary>
        /// <param name="caller">The parent caller.</param>
        public BrowserCallbackForJs(BrowserForm caller)
        {
            this.caller = caller;
        }

        /// <summary>
        /// Logs a message to a file.
        /// </summary>
        /// <param name="message">The message to Log.</param>
        public void LogMessage(string message)
        {
            ILog log = LogManager.GetLogger("Browser");
            log.Info(message);
        }

        public void SetServerStatus(string input)
        {
            IList<ServerStatus> list = JsonConvert.DeserializeObject<List<ServerStatus>>(input);
            caller.SetData(list);
        }
    }
}
